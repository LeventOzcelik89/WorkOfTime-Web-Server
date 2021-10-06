using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    
    public class VMPRD_TitanDeviceActivated:PRD_TitanDeviceActivated
    {
        private TitanServices TitanServices = new TitanServices();
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public IGeometry Location { get; set; }
        public VMPRD_TitanDeviceActivated Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            if (this.SerialNumber!=null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedBySerialCode(this.SerialNumber)??this);
            }
            else if (this.InventoryId!=null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedByInventoryId(this.InventoryId.Value) ?? this);
            }
            else if (this.id != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedById(this.id) ?? this);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetPRD_TitanDeviceActivatedById(this.id);
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
            var getall=(DeviceResultList)GetAllDevices().objects;
            var dbresult = db.InsertPRD_TitanDeviceActivated(this, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Başarıyla Kaydedildi"
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Başarısız Oldu"
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdatePRD_TitanDeviceActivated(this, false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
          
            var dbresult = db.DeletePRD_TitanDeviceActivated(this.id, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Silme İşlemi Başarılı oldu"
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Silme İşlemi Başarısız Oldu"
                };
            }
        }
        public ResultStatus GetAllDevices()
        {
            return TitanServices.GetAllDevices();
        }
        public ResultStatus GetDeviceById(Guid id)
        {
            return TitanServices.GetDeviceById(id);
        }
        public ResultStatus GetDeviceInformation(Guid id)
        {
            return TitanServices.GetDeviceInformation(id);
        }
        public ResultStatus GetDeviceActivationInformation()
        {
            return TitanServices.GetDeviceActivationInformations();
        }
    }
}
