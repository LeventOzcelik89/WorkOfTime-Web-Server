using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSV_ServiceOperationModel : VWSV_ServiceOperation
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<VWPRD_ProductionProduct> expens { get; set; }
        public VWPRD_Transaction Transaction { get; set; }
        public List<VWPRD_ProductMateriel> TreeProduct { get; set; } = new List<VWPRD_ProductMateriel>();
        public List<VWPRD_Product> ServiceNotifications { get; set; }
        public short Type { get; set; }
        public Guid? storageId { get; set; }
        public VMSV_ServiceOperationModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_ServiceOperationById(this.id);
            if (serviceId != null)
            {
                var findService = db.GetVWSV_ServiceById(serviceId.Value);
                if (findService != null)
                {
                    var serviceModel = new VMSV_ServiceModel();
                    serviceModel.GetVWPRD_ProductMateriels(findService.productId.HasValue ? findService.productId.Value : new Guid());
                    TreeProduct = serviceModel.GetProductMetarials;
                }
            }
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWSV_ServiceOperationById(this.id);
            var res = new ResultStatus { result = true };
            var validation = Validator();
            if (validation.result == false)
            {
                return validation;
            }
            if (data == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                res = Insert();
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                res = Update();
            }
            if (res.result)
            {
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
            db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
            return res;
        }
        private ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
            var getService = new VMSV_ServiceModel { id = serviceId.Value }.Load();
            if (this.status == (short)EnumSV_ServiceOperation.AskCustomer)
            {
                SendAskCustomerMail();
            }
            if (this.CompanyId.HasValue&&this.deliveryType.HasValue)
            {
                var getCompany = db.GetVWCMP_CompanyById(this.CompanyId.Value);
                if (getCompany != null)
                {

                    description = $"Transfer Edilen Şube: {getCompany.fullName} </br></br>" + description;
                }

                getService.stage = (short)EnumSV_ServiceStages.DeviceHanded;//süreç başa döner
                res &= db.UpdateSV_Service(getService.B_ConvertType<SV_Service>(), false, trans);
                var getProduct = db.GetVWPRD_ProductById(getService.productId.Value);
                var getInventory = db.GetVWPRD_InventoryById(getService.inventoryId.Value);
                var transItem = new VMPRD_TransactionItems
                {
                    productId = getInventory.productId,
                    quantity = 1,
                    serialCodes = getInventory.serialcode ?? "",
                    unitPrice = getProduct.currentSellingPrice,

                };
                var transModelCompanyId = new VMPRD_TransactionModel
                {
                    inputId = db.GetCMP_StorageByCompanyIdFirst(CompanyId.Value)?.id,
                    inputTable = "CMP_Storage",
                    inputCompanyId = CompanyId,
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
                res &= transModelCompanyId.Save(this.createdby, trans);

            }
            res &= db.InsertSV_ServiceOperation(this.B_ConvertType<SV_ServiceOperation>(), this.trans);
            if (!res.result)
            {
                Log.Error(res.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Servis Operasyonu oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Servis Operasyonu oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        private void SendAskCustomerMail()
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = new VMSV_ServiceModel { id = this.serviceId.Value }.Load();
            var imei = db.GetPRD_InventoryById(data.inventoryId.Value);
            if (data.Customer.email != null)
            {
                var text = "<h3>Sayın " + data.Customer.fullName + "</h3>";
                text += "<p>" + imei.serialcode + " kodlu cihaz için teknik servis bedelleri ektedir.</p>";

                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Teknik Servis Bildirimi Hakkında", text).Send((Int16)EmailSendTypes.ZorunluMailler, data.Customer.email, $"{TenantConfig.Tenant.TenantName } | Teknik Servis Bildirimi", true, null, null, new string[] { $"{TenantConfig.Tenant.GetWebUrl()}/SV/VWSV_Service/PrintEnd?id={data.id}" }, false);
            }

        }
        private void SendDoneMail()
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = new VMSV_ServiceModel { id = this.serviceId.Value }.Load();
            var imei = db.GetPRD_InventoryById(data.inventoryId.Value);
            if (data.Customer.email != null)
            {
                var text = "<h3>Sayın " + data.Customer.fullName + "</h3>";
                text += "<p>" + imei.serialcode + " kodlu cihaz için yapılan işlemler tamamlanmıştır.</p>";

                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Teknik Servis Bildirimi Hakkında", text).Send((Int16)EmailSendTypes.ZorunluMailler, data.Customer.email, $"{TenantConfig.Tenant.TenantName } | Teknik Servis Bildirimi", true, null, null, new string[] { $"{TenantConfig.Tenant.GetWebUrl()}/SV/VWSV_Service/PrintEnd?id={data.id}" }, false);
            }

        }

        public ResultStatus Upsert(Guid userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = trans ?? db.BeginTransaction();
            var user = db.GetVWSH_UserById(userId);
            var result = new ResultStatus { result = true };
            if (expens != null)
            {
                var expensItem = this.expens.Select(x => new VMPRD_TransactionItems
                {
                    productId = x.productId,
                    quantity = x.quantity,
                    serialCodes = x.serialCodes ?? null,
                    unitPrice = db.GetVWPRD_ProductById(x.productId.Value)?.currentSellingPrice,
                }).ToList();
                var transId = Guid.NewGuid();
                var transModel = new VMPRD_TransactionModel
                {
                    inputId = null,
                    inputTable = null,
                    outputTable = "CMP_Storage",
                    outputCompanyId = user.CompanyId,
                    outputId = this.storageId,
                    created = DateTime.Now,
                    createdby = userId,
                    status = (int)EnumPRD_TransactionStatus.islendi,
                    items = expensItem,
                    date = DateTime.Now,
                    code = BusinessExtensions.B_GetIdCode(),
                    type = this.Transaction.type,
                    id = transId

                };
                result &= transModel.Save(userId, trans);
                result &= new VMSV_ServiceOperationModel
                {
                    created = DateTime.Now,
                    createdby = userId,
                    dataId = transId,
                    dataTable = "PRD_Transaction",
                    description = this.description,
                    serviceId = this.serviceId,
                    status = this.Transaction.type == (short)EnumPRD_TransactionType.HarcamaBildirimi ? (short)EnumSV_ServiceOperation.HarcamaBildirildi : (short)EnumSV_ServiceOperation.FireBildirimiYapildi,
                }.Save(userId, null, this.trans);
            }

            if (this.ServiceNotifications != null)
            {
                foreach (var item in ServiceNotifications)
                {
                    result &= new VMSV_ServiceOperationModel
                    {
                        created = DateTime.Now,
                        createdby = this.createdby,
                        dataId = item.id,
                        description = db.GetVWPRD_ProductById(item.id)?.currentSellingPrice.ToString() ?? "",
                        dataTable = "PRD_Product",
                        serviceId = this.serviceId,
                        status = (short)EnumSV_ServiceOperation.ServicePriceAdded
                    }.Save(this.createdby, null, this.trans);
                }
            }

            if (result.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            return result;
        }

        public ResultStatus Cargo(Guid userId)
        {
            if (!this.serviceId.HasValue)
            {
                return new ResultStatus { result = false };
            }
            var result = new ResultStatus { result = true };
            db = db ?? new WorkOfTimeDatabase();
            trans = trans ?? db.BeginTransaction();
            
            this.status = (short)EnumSV_ServiceOperation.Done;
            var getService = db.GetVWSV_ServiceById(this.serviceId.Value);
            var getInventory = db.GetVWPRD_InventoryById(getService.inventoryId.Value);
            
            var transItem = new VMPRD_TransactionItems
            {
                productId = getInventory.productId,
                quantity = 1,
                serialCodes = getInventory.serialcode ?? "",
                unitPrice = 0,
            };
            if (CompanyId.HasValue)
            {
                var transModelCompanyId = new VMPRD_TransactionModel
                {
                    inputId = db.GetCMP_StorageByCompanyIdFirst(CompanyId.Value)?.id,
                    inputTable = "CMP_Storage",
                    inputCompanyId = CompanyId,
                    outputCompanyId = getInventory.lastActionDataCompanyId,
                    outputId = getInventory.lastActionDataId,
                    created = this.created,
                    createdby = this.createdby,
                    status = (int)EnumPRD_TransactionStatus.islendi,
                    items = new List<VMPRD_TransactionItems> { transItem },
                    date = DateTime.Now,
                    code = BusinessExtensions.B_GetIdCode(),
                    type = (short)EnumPRD_TransactionType.TeknikServisCikis,
                    id = Guid.NewGuid(),
                };
                this.dataTable = "PRD_Transaction";
                this.dataId = transModelCompanyId.id;
                result &= transModelCompanyId.Save(this.createdby, trans);
            }
            result &=  this.Save(userId, null, trans);
            if (result.result)
            {
                this.SendDoneMail();
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            return result;
        }

        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSV_ServiceOperation(this.B_ConvertType<SV_ServiceOperation>(), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Servis Operasyonu güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Servis Operasyonu güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSV_ServiceOperation(this.id, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Servis Operasyonu silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Servis Operasyonu silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus GetStockByProductAndStorageId(Guid[] productId, Guid storageId)
        {
            var result = new ResultStatus { result = true };
            db = db ?? new WorkOfTimeDatabase();
            result.objects = db.GetVWPRD_StockSummaryByProductIdsAndStockId(productId, storageId);
            return result;
        }
        public ResultStatus QualiltyCheck(Guid serviceId, bool status, Guid userId)
        {
            var result = new ResultStatus { result = true };
            db = db ?? new WorkOfTimeDatabase();
            trans = trans ?? db.BeginTransaction();
            if (status == true)
            {
                result &= new VMSV_ServiceModel { id = serviceId }.NextStage(userId, trans);
                result &= new VMSV_ServiceOperationModel
                {
                    created = this.created,
                    createdby = userId,
                    description = "Kalite Kontrol Başarılı",
                    serviceId = serviceId,
                    status = (int)EnumSV_ServiceOperation.QualityControl,
                }.Save(userId, null, this.trans);
            }
            else
            {
                var service = new VMSV_ServiceModel { id = serviceId }.Load();
                service.stage = (int)EnumSV_ServiceStages.Fixing;
                result &= service.Save(userId, null, trans);
                result &= new VMSV_ServiceOperationModel
                {
                    created = this.created,
                    createdby = userId,
                    description = "Kalite Kontrol Başarısız",
                    serviceId = serviceId,
                    status = (int)EnumSV_ServiceOperation.QualityControlNot,
                }.Save(userId, null, this.trans);
            }
            if (result.result)
            {
                trans.Commit();
            }
            else
            {
                Log.Error(result.message);
                trans.Rollback();
            }
            return result;
        }
    }
}
