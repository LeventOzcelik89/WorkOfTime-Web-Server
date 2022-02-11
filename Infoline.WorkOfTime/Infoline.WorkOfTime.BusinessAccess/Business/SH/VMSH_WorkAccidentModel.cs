using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSH_WorkAccidentModel : VWSH_WorkAccident
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public string taskCode { get; set; }
        public VMSH_WorkAccidentModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSH_WorkAccidentById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
                if (this.taskId.HasValue)
                {
                    var task = db.GetVWFTM_TaskById(this.taskId.Value);
                    this.taskCode = task.code ?? "-";
                }
                else
                {
                    this.taskCode = "-";
                }
            }
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWSH_WorkAccidentById(this.id);
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

            var dbresult = db.InsertSH_WorkAccident(new SH_WorkAccident().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Bildirim Formu Oluşturma İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kaza ve Olay Bildirim Formu Oluşturma İşlemi Başarıyla Gerçekleşti."
                };
            }
        }
        private ResultStatus Update()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSH_WorkAccident(new SH_WorkAccident().B_EntityDataCopyForMaterial(this), true, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Bildirim Formu Güncelleme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kaza ve Olay Bildirim Formu Güncelleme İşlemi Başarıyla Gerçekleşti."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();

            var dbresult = db.DeleteSH_WorkAccident(new SH_WorkAccident { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Bildirim Formu Silme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Kaza ve Olay Bildirim Formu Silme İşlemi Başarıyla Gerçekleştirildi."
                };
            }
        }
       
    }

}
