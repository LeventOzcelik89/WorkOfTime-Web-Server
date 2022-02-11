using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSH_CorrectiveActivityModel : VWSH_CorrectiveActivity
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMSH_CorrectiveActivityModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSH_CorrectiveActivityById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWSH_CorrectiveActivityById(this.id);
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
            //db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
          
            return res;
        }
        private ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };

            var dbresult = db.InsertSH_CorrectiveActivity(new SH_CorrectiveActivity().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Düzenleyici ve Önleyici Faaliyet Formu Oluşturma İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Düzenleyici ve Önleyici Faaliyet Formu Oluşturma İşlemi Başarıyla Gerçekleşti."
                };
            }
        }
        private ResultStatus Update()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSH_CorrectiveActivity(new SH_CorrectiveActivity().B_EntityDataCopyForMaterial(this), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Düzenleyici ve Önleyici Faaliyet Formu Güncelleme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Düzenleyici ve Önleyici Faaliyet Formu Güncelleme İşlemi Başarıyla Gerçekleşti."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();

            var dbresult = db.DeleteSH_CorrectiveActivity(new SH_CorrectiveActivity { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Düzenleyici ve Önleyici Faaliyet Formu Silme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Düzenleyici ve Önleyici Faaliyet Formu Silme İşlemi Başarıyla Gerçekleştirildi."
                };
            }
        }
       
    }

}
