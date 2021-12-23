using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSV_DeviceCameWithModel : VWSV_DeviceCameWith
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMSV_DeviceCameWithModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_DeviceCameWithById(this.id);
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
            var data = db.GetVWSV_DeviceCameWithById(this.id);
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
            //Validasyonlarını yap
            var dbresult = db.InsertSV_DeviceCameWith(this.B_ConvertType<SV_DeviceCameWith>(), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Cihaz Yanında Gelen Ürün oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Cihaz Yanında Gelen Ürün oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSV_DeviceCameWith(this.B_ConvertType<SV_DeviceCameWith>(), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Cihaz Yanında Gelen Ürün güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Cihaz Yanında Gelen Ürün güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSV_DeviceCameWith(this.id, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Cihaz Yanında Gelen Ürün silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Cihaz Yanında Gelen Ürün silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

    }
}
