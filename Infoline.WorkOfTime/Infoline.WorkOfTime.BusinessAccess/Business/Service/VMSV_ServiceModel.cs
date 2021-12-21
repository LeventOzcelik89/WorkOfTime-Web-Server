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
        public int DeliveryTpeActual { get; set; }
        public VWSV_Customer Customer { get; set; }
        public VMSV_ServiceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_ServiceById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            var res = new ResultStatus { result = true };
            this.db = this.db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWSV_ServiceById(this.id);
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
                    activation = findTitan.CreatedOfTitan.HasValue ? findTitan.CreatedOfTitan.Value.ToShortDateString() : "Cihaz Aktif Edilmemiştir",
                    deviceName = findInventory.fullNameProduct,
                    dist = findDist != null ? findDist.DistributorName : "",
                    company = findDist != null ? findDist.CustomerOperatorName : "",
                    manifacturDate = findManiDate.Length > 0 ? findManiDate.FirstOrDefault().created.Value.ToShortDateString() : "",
                    sellingDate=findSelling!=null?findSelling.contractStartDate.HasValue?findSelling.contractStartDate.Value.ToShortDateString():"":""   
                };
           
            return new ResultStatus { result = true, objects = titan };
        }
    }
}
