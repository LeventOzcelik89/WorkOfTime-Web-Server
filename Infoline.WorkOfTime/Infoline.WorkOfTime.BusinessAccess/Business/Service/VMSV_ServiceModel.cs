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
        public string Url { get; set; }
        public string Logo { get; set; }
        public List<VWPRD_ProductMateriel> GetProductMetarials { get; set; } = new List<VWPRD_ProductMateriel>();
        public Guid ProductId { get; set; }
        public VMSV_ServiceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_ServiceById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
                var customerService = db.GetVWSV_CustomerUserByServiceId(this.id);
                var customer = db.GetVWSV_CustomerById(customerService.customerId.Value);
                var findTitan = db.GetPRD_TitanDeviceActivatedByInventoryId(this.inventoryId.Value);
                if (findTitan != null)
                {
                    if (findTitan.CreatedOfTitan.HasValue)
                    {
                        if (findTitan.CreatedOfTitan.Value.AddYears(2) <= DateTime.Now)
                        {
                            WarrantyStatus = "Garantili";
                        }
                        WarrantyStart = findTitan.CreatedOfTitan.Value.ToShortDateString();
                        WarrantyEnd = findTitan.CreatedOfTitan.Value.AddDays(2).ToShortDateString();
                    }
                }
                this.Customer = new VMSV_CustomerModel().B_EntityDataCopyForMaterial(customer);
                var getProblems = db.GetVWSV_DeviceProblemsByServiceId(this.id);
                Problems = getProblems.Select(x => new VMSV_DeviceProblemModel().B_EntityDataCopyForMaterial(x)).ToList();
                var getCameWith = db.GetVWSV_DeviceCameWithByServiceId(this.id).Select(x => new VMSV_DeviceCameWithModel().B_EntityDataCopyForMaterial(x));
                this.CameWith = getCameWith.ToList();
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
            var result = db.InsertSV_Service(this.B_ConvertType<SV_Service>(), this.trans);
            result &= Customer.Save(this.createdby, null, this.trans);
            result &= new VMSV_CustomerUserModel { customerId = Customer.id, serviceId = this.id, type = (int)EnumSV_CustomerUser.Other }.Save(this.createdby, null, this.trans);
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
                description = "Servis Kaydı Oluşturuldu",
                serviceId = this.id,
                status = (int)EnumSV_ServiceOperation.Started,
                userId = this.createdby
            }.Save(this.createdby, null, this.trans);
            if (result.result)
            {
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
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSV_Service(this.id, trans);
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
                        this.GetVWPRD_ProductMateriels(item.productId.Value);
                    }
                }
            }
            return this;
        }

        public List<VWPRD_ProductMateriel> GetProducts(Guid productId)
        {

            db = db ?? new WorkOfTimeDatabase();

            var prodMaterials = db.GetPRD_ProductMaterielByMaterialId(productId);
            foreach (var mat in prodMaterials)
            {

            }

            return
 new List<VWPRD_ProductMateriel>();
        }


    }
}
