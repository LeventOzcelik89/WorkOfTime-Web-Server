using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSV_ServiceModel : VWSV_Service
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public int? DeliveryTpeActual { get; set; }
        public VMSV_CustomerModel Customer { get; set; }
        public List<VMSV_DeviceProblemModel> Problems { get; set; }
        public List<VMSV_DeviceCameWithModel> CameWith { get; set; }
        public string WarrantyStatus { get; set; } = "Garanti Dışı";
        public string WarrantyStart { get; set; } = "Cihaz Aktif Edilmemiştir";
        public string WarrantyEnd { get; set; } = "Cihaz Aktif Edilmemiştir";
        public string ProducedDate { get; set; } = "Cihaz Aktif Edilmemiştir";
        public List<VWPRD_ProductMateriel> GetProductMetarials { get; set; } = new List<VWPRD_ProductMateriel>();
        public List<VWSV_ServiceOperation> ServiceOperations { get; set; } = new List<VWSV_ServiceOperation>();
        public VWPRD_EntegrationImport EntegrationImport { get; set; } = new VWPRD_EntegrationImport();
        public PRD_EntegrationAction EntegrationAction { get; set; } = new PRD_EntegrationAction();
        public PRD_TitanDeviceActivated PRD_TitanDeviceActivated { get; set; } = new PRD_TitanDeviceActivated();
        public VWPRD_Inventory VWPRD_Inventory { get; set; } = new VWPRD_Inventory();
        public List<VWPRD_TransactionItem> WastageProducts { get; set; } = new List<VWPRD_TransactionItem>();
        public List<VWPRD_TransactionItem> SpendedProducts { get; set; } = new List<VWPRD_TransactionItem>();
        public double TotalWasted { get; set; } = 0;
        public double TotalSpended { get; set; } = 0;
        public List<EnumSV_ServiceActions> ButtonPermission { get; set; } = new List<EnumSV_ServiceActions>();

        public VMSV_ServiceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_ServiceById(this.id);
            if (data != null)
            {

                this.B_EntityDataCopyForMaterial(data, true);
                var findInventory = db.GetVWPRD_InventoryById(this.inventoryId.Value);
                var customerService = db.GetVWSV_CustomerUserByServiceId(this.id);
                var customer = db.GetVWSV_CustomerById(customerService.customerId.Value);
                var findTitan = db.GetPRD_TitanDeviceActivatedByInventoryId(this.inventoryId.Value);
                if (findTitan != null)
                {
                    PRD_TitanDeviceActivated = findTitan;
                    if (findTitan.CreatedOfTitan.HasValue)
                    {
                        if (findTitan.CreatedOfTitan.Value.AddYears(2) >= DateTime.Now)
                        {
                            WarrantyStatus = "Garantili";
                        }
                        WarrantyStart = findTitan.CreatedOfTitan.Value.ToShortDateString();
                        WarrantyEnd = findTitan.CreatedOfTitan.Value.AddYears(2).ToShortDateString();
                    }
                }
                this.EntegrationImport = db.GetVWPRD_EntegrationImportBySerialCode(findInventory.serialcode) ?? new VWPRD_EntegrationImport();
                this.EntegrationAction = db.GetPRD_EntegrationActionBySerialNumbersOrImei(findInventory.serialcode) ?? new PRD_EntegrationAction(); ;
                this.VWPRD_Inventory = findInventory;
                var findManiDate = db.GetPRD_InventoryActionByInventoryId(inventoryId.Value);
                ProducedDate = findManiDate.Where(x => x.type == (int)EnumPRD_InventoryActionType.Uretildi).FirstOrDefault().created.Value.ToShortDateString();
                this.Customer = new VMSV_CustomerModel().B_EntityDataCopyForMaterial(customer);
                var getProblems = db.GetVWSV_DeviceProblemsByServiceId(this.id);
                Problems = getProblems.Select(x => new VMSV_DeviceProblemModel().B_EntityDataCopyForMaterial(x)).ToList();
                var getCameWith = db.GetVWSV_DeviceCameWithByServiceId(this.id).Select(x => new VMSV_DeviceCameWithModel().B_EntityDataCopyForMaterial(x));
                this.CameWith = getCameWith.ToList();
                this.SetButtonPermission();
                this.ServiceOperations = db.GetVWSV_ServiceOperationsByIdServiceId(this.id).ToList() ?? new List<VWSV_ServiceOperation>();
                if (ServiceOperations.Count>0)
                {
                    var spend =  GetSpendedProducts(id);
                    var wasted =GetWastedProducts(id);
                    TotalSpended= spend.Sum(x => x.unitPrice * x.quantity) ?? 0;
                    TotalWasted = wasted.Sum(x => x.unitPrice * x.quantity) ?? 0;
                    
                }
            }
            return this;
        }
        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            var res = new ResultStatus { result = true };
            this.db = this.db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWSV_ServiceById(this.id);
            if (DeliveryTpeActual.HasValue)
            {
                this.deliveryType = (short?)DeliveryTpeActual;
            }
            if (data == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                res &= Insert();
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                res &= Update();
            }
            if (res.result)
            {
                if (request != null)
                {
                    new FileUploadSave(request, this.id, "SV_Service").SaveAs();
                }
                if (transaction == null) trans.Commit();
            }
            else
            {
                if (transaction == null) trans.Rollback();
            }
            return res;
        }
        private ResultStatus Validator()
        {
            //Daha onceden aynı kod var mı ? 
            this.db = this.db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
            var isExistCodeBefore = db.GetVWSV_ServiceByCodeAndIdIsNotEqual(this.code, this.id);
            if (isExistCodeBefore != null)
            {
                res.result = false;
            }
            return res;
        }
        private ResultStatus Insert()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.stage = (int)EnumSV_ServiceStages.DeviceHanded;
            var result = db.InsertSV_Service(this.B_ConvertType<SV_Service>(), this.trans);
            result &= Customer.Save(this.createdby, null, this.trans);
            var getInventory = db.GetVWPRD_InventoryById(this.inventoryId.Value);
            result &= new VMSV_CustomerUserModel { customerId = Customer.id, serviceId = this.id, type = (int)EnumSV_CustomerUser.Other }.Save(this.createdby, null, this.trans);
            var user = db.GetVWSH_UserById(this.createdby.Value);
            var transItem = new VMPRD_TransactionItems
            {
                productId = getInventory.productId,
                quantity = 1,
                serialCodes = getInventory.serialcode ?? "",
                unitPrice = 0,
            };
            if (companyId.HasValue)
            {
                var transModelCompanyId = new VMPRD_TransactionModel
                {
                    inputId = db.GetCMP_StorageByCompanyIdFirst(companyId.Value)?.id,
                    inputTable = "CMP_Storage",
                    inputCompanyId = companyId,
                    outputCompanyId = getInventory.lastActionDataCompanyId,
                    outputId =getInventory.lastActionDataId,
                    created = this.created,
                    createdby = this.createdby,
                    status = (int)EnumPRD_TransactionStatus.islendi,
                    items = new List<VMPRD_TransactionItems> { transItem },
                    date = DateTime.Now,
                    code = BusinessExtensions.B_GetIdCode(),
                    type = (short)EnumPRD_TransactionType.TeknikServisTransferi,
                    id = Guid.NewGuid(),
                };
                result &= transModelCompanyId.Save(this.createdby, trans);
            }
            var transModel = new VMPRD_TransactionModel
            {
                inputId = db.GetCMP_StorageByCompanyIdFirst(user.CompanyId.Value)?.id,
                inputTable = "CMP_Storage",
                inputCompanyId = user.CompanyId,
                outputCompanyId = getInventory.lastActionDataCompanyId,
                outputId = getInventory.lastActionDataId,
                created = this.created,
                createdby = this.createdby,
                status = (int)EnumPRD_TransactionStatus.islendi,
                items = new List<VMPRD_TransactionItems> { transItem },
                date = DateTime.Now,
                code = BusinessExtensions.B_GetIdCode(),
                type = (short)EnumPRD_TransactionType.TeknikServisTransferi,
                id = Guid.NewGuid(),
            };
            result &= transModel.Save(this.createdby, trans);
            if (Problems != null)
            {
                foreach (var problem in Problems)
                {
                    problem.serviceId = this.id;
                    problem.type = (int)EnumSV_DeviceProblemType.Customer;
                    result &= problem.Save(this.createdby, null, this.trans);
                }
            }
            if (CameWith != null)
            {
                foreach (var Coming in CameWith)
                {
                    Coming.serviceId = this.id;
                    result &= Coming.Save(this.createdby, null, this.trans);
                }
            }
            result &= new VMSV_ServiceOperationModel
            {
                created = this.created,
                createdby = this.createdby,
                description = "Servis Kaydı Oluşturuldu",
                serviceId = this.id,
                status = (int)EnumSV_ServiceOperation.Started,
            }.Save(this.createdby, null, this.trans);

            if (result.result)
            {
                SendMail();
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Servis Kaydı Oluşturma İşlemi Başarılı"
                };
            }
            else
            {
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Servis Kaydı Oluşturma İşlemi Başarısız Oldu"
                };
            }
        }
        private ResultStatus Update()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var result = db.UpdateSV_Service(this.B_ConvertType<SV_Service>(), true, this.trans);
            result &= Customer.Save(this.createdby, null, this.trans);
            var getAllProblems = db.GetVWSV_DeviceProblemsByServiceId(this.id);
            var getAllCameWith = db.GetVWSV_DeviceCameWithByServiceId(this.id);
            result &= db.BulkDeleteSV_DeviceProblem(getAllProblems.Select(x => new SV_DeviceProblem { id = x.id }), trans);
            result &= db.BulkDeleteSV_DeviceCameWith(getAllCameWith.Select(x => new SV_DeviceCameWith { id = x.id }), trans);
            if (Problems != null)
            {
                foreach (var problem in Problems)
                {
                    problem.serviceId = this.id;
                    result &= problem.Save(this.createdby, null, this.trans);
                }
            }
            if (CameWith != null)
            {
                foreach (var Coming in CameWith)
                {
                    Coming.serviceId = this.id;
                    result &= Coming.Save(this.createdby, null, this.trans);
                }
            }
            result &= new VMSV_ServiceOperationModel
            {
                created = this.created,
                createdby = this.createdby,
                description = "Servis Kaydı Güncellendi",
                serviceId = this.id,
                status = (int)EnumSV_ServiceOperation.Updated,
            }.Save(this.createdby, null, this.trans);
            if (result.result)
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Servis Kaydı Güncelleme İşlemi Başarılı"
                };
            }
            else
            {
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Servis Kaydı Güncelleme İşlemi Başarısız Oldu"
                };
            }
        }
        public ResultStatus DeviceInformation(Guid inventoryId)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var findInventory = db.GetVWPRD_InventoryById(inventoryId);
            if (findInventory == null)
            {
                return new ResultStatus
                {
                    message = "Böyle bir cihaz yoktur!",
                    result = false
                };
            }
            var findTitan = db.GetPRD_TitanDeviceActivatedByInventoryId(inventoryId);
            var findDist = db.GetPRD_EntegrationActionBySerialNumbersOrImei(findInventory.serialcode);
            var findManiDate = db.GetPRD_InventoryActionByInventoryId(inventoryId);
            var findSelling = db.GetVWPRD_EntegrationImportBySerialCode(findInventory.serialcode);
            findManiDate = findManiDate.Where(x => x.type == (int)EnumPRD_InventoryActionType.Uretildi).ToArray();
            findTitan = findTitan ?? new PRD_TitanDeviceActivated();
            object titan = new
            {
                warranty = findTitan.CreatedOfTitan.HasValue ? findTitan.CreatedOfTitan.Value.AddYears(2).ToShortDateString() : "Cihaz Aktif Edilmemiştir",
                warrantyStatus = findTitan.CreatedOfTitan.HasValue ? findTitan.CreatedOfTitan.Value.AddYears(2) <= DateTime.Now ? "Garantili" : "Garanti Dışı" : "Garanti Dışı",
                activation = findTitan.CreatedOfTitan.HasValue ? findTitan.CreatedOfTitan.Value.ToShortDateString() : "Cihaz Aktif Edilmemiştir",
                deviceName = findInventory.fullNameProduct,
                deviceId = findInventory.productId.HasValue ? findInventory.productId.ToString() : "",
                dist = findDist != null ? findDist.DistributorName : "",
                company = findDist != null ? findDist.CustomerOperatorName : "",
                manifacturDate = findManiDate.Length > 0 ? findManiDate.FirstOrDefault().created.Value.ToShortDateString() : "",
                sellingDate = findSelling != null ? findSelling.contractStartDate.HasValue ? findSelling.contractStartDate.Value.ToShortDateString() : "" : ""
            };
            return new ResultStatus { result = true, objects = titan };
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var getAllProblems = db.GetVWSV_DeviceProblemsByServiceId(this.id);
            var getAllCameWith = db.GetVWSV_DeviceCameWithByServiceId(this.id);
            var getAllServiceUser = db.GetVWSV_CustomerUserByServiceId(this.id);
            var getlAllActions = db.GetVWSV_ServiceOperationsByIdServiceId(this.id);
            dbresult = db.BulkDeleteSV_DeviceProblem(getAllProblems.Select(x => new SV_DeviceProblem { id = x.id }), trans);
            dbresult &= db.BulkDeleteSV_DeviceCameWith(getAllCameWith.Select(x => new SV_DeviceCameWith { id = x.id }), trans);
            dbresult &= db.DeleteSV_CustomerUser(getAllServiceUser.id, trans);
            dbresult &= db.BulkDeleteSV_ServiceOperation(getlAllActions.Select(x => new SV_ServiceOperation { id = x.id }), trans);
            dbresult &= db.DeleteSV_Service(this.id, trans);

            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Servis Kaydı silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Servis Kaydı silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        public VMSV_ServiceModel GetVWPRD_ProductMateriels(Guid productId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var findProduct = db.GetVWPRD_ProductById(productId);
            if (findProduct != null)
            {
                var findMetarials = db.GetVWPRD_ProductMaterialByProductId(productId);
                if (findMetarials.Length > 0)
                {
                    this.GetProductMetarials.AddRange(findMetarials);
                    foreach (var item in findMetarials)
                    {
                        GetVWPRD_ProductMateriels(item.materialId.Value);
                    }
                }
            }
            return this;
        }
        public void SendMail(int type=0)
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = this.Load();
            var imei = db.GetPRD_InventoryById(data.inventoryId.Value);
            if (data.Customer.email != null)
            {
                var text = "<h3>Sayın " + data.Customer.fullName + "</h3>";
                if (type==1)
                {

                    text += "<p>Müşterinin Onayına Sun</p>";
                  
                }
                else
                {

                    text += "<p>" + imei.serialcode + " kodlu cihaz teknik servisimize gelmiştir</p>";
                 
                }
                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "satinalma.jpg", TenantConfig.Tenant.TenantName + " | WorkOfTime | Teknik Servis", text).Send((Int16)EmailSendTypes.SatinAlma, data.Customer.email, "Teknik Servis", true);
            }





        }
        public void SetButtonPermission()
        {
            if (stage == null)
            {
                return;
            }
            if (lastOperationStatus == (int)EnumSV_ServiceOperation.BeginTransfer)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.TransferEnds);
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.Canceled)
            {

            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.Waiting)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.ReStart);
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.Restart)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.TransferEnded)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.AsamaBildirimi)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.FireBildirimiYapildi)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.HarcamaBildirildi)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.Updated)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.Done)
            {

            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.Started)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else if (lastOperationStatus == (int)EnumSV_ServiceOperation.PartChanged)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }
            else
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                SetStageButton();
            }

        }
        private void SetStageButton()
        {
            if (stage == (int)EnumSV_ServiceStages.DeviceHanded)
            {

                ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
                ButtonPermission.Add(EnumSV_ServiceActions.NextStage);
            }
            else if (stage == (int)EnumSV_ServiceStages.Detection)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
                ButtonPermission.Add(EnumSV_ServiceActions.ChancingPart);
                ButtonPermission.Add(EnumSV_ServiceActions.NextStage);
            }
            else if (stage == (int)EnumSV_ServiceStages.UserPermission)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
                //ButtonPermission.Add(EnumSV_ServiceActions.AskCustomer);
                ButtonPermission.Add(EnumSV_ServiceActions.NextStage);

            }
            else if (stage == (int)EnumSV_ServiceStages.Fixing)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
                ButtonPermission.Add(EnumSV_ServiceActions.Fire);
                ButtonPermission.Add(EnumSV_ServiceActions.Harcama);
                ButtonPermission.Add(EnumSV_ServiceActions.NewImei);
                ButtonPermission.Add(EnumSV_ServiceActions.NextStage);

            }
            //else if (stage == (int)EnumSV_ServiceStages.Qualitycontrol)
            //{
            //    ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
            //    ButtonPermission.Add(EnumSV_ServiceActions.QualityControllNot);
            //    ButtonPermission.Add(EnumSV_ServiceActions.NextStage);

            //}
            else if(stage == (int)EnumSV_ServiceStages.Delivery)
            {
                ButtonPermission.Add(EnumSV_ServiceActions.Cancel);
                //ButtonPermission.Add(EnumSV_ServiceActions.TransferStart);
                //ButtonPermission.Add(EnumSV_ServiceActions.Stop);
                ButtonPermission.Add(EnumSV_ServiceActions.Done);
            }
            else
            {

            }
        }
        public ResultStatus NextStage(Guid userId, DbTransaction trans = null)
        {

            var result = new ResultStatus { result = true };
            this.Load();
            if (this.stage == (int)EnumSV_ServiceStages.Delivery)
            {
                return new ResultStatus { result = false, message = "Cihaz Teslim Edilmiştir" };
            }
            if (this.stage==(int)EnumSV_ServiceStages.Detection&&!this.Problems.Any(x=>x.type==(short)EnumSV_DeviceProblemType.Service&&x.warranty==false))
            {
                this.stage++;
            }
            this.stage++;
            this.Save(userId, null, trans);
            return result;



        }
        public List<VWPRD_TransactionItem> GetWastedProducts(Guid serviceId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var getAllServiceAction = db.GetVWSV_ServiceOperationsByIdServiceId(serviceId).Where(x=>x.status==(short)EnumSV_ServiceOperation.FireBildirimiYapildi).ToList();
            if (getAllServiceAction!=null)
            {
                var allItems = db.GetVWPRD_TransactionItemByTransactionIds(getAllServiceAction.Select(x=>x.dataId.Value).ToArray());
                if (allItems.Length<=0)
                {
                    return new List<VWPRD_TransactionItem>();
                }
                this.WastageProducts = allItems.Select(x=> { x.id = x.transactionId.Value; return x; }).ToList();
                
            }
            return WastageProducts;

        }
        public List<VWPRD_TransactionItem> GetSpendedProducts(Guid serviceId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var getAllServiceAction = db.GetVWSV_ServiceOperationsByIdServiceId(serviceId).Where(x => x.status == (short)EnumSV_ServiceOperation.HarcamaBildirildi).ToList();
            if (getAllServiceAction != null)
            {
                var allItems = db.GetVWPRD_TransactionItemByTransactionIds(getAllServiceAction.Select(x => x.dataId.Value).ToArray());
                if (allItems.Length <= 0)
                {
                    return new List<VWPRD_TransactionItem>();
                }
                this.SpendedProducts = allItems.Select(x => { x.id = x.transactionId.Value; return x; }).ToList();

            }
            return SpendedProducts;

        }
    }
}
