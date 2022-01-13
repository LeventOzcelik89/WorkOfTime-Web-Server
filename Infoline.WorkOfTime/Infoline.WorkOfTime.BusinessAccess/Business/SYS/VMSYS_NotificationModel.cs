using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSYS_NotificationModel:VWSYS_Notification
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMSYS_NotificationModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSYS_NotificationById(this.id);
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
            var data = db.GetVWSYS_NotificationById(this.id);
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
            var dbresult = db.InsertSYS_Notification(this.B_ConvertType<SYS_Notification>(), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Bildirim Gönderme İşlemi Başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Bildirim Gönderme İşlemi Başarılı Şekilde Gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSYS_Notification(this.B_ConvertType<SYS_Notification>(), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Bildirim Gönderme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Bildirim Gönderme İşlemi Başarılı Şekilde Gerçekleşti."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSYS_Notification(this.id, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Bildirim Gönderme silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Bildirim Gönderme silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
    }
}
