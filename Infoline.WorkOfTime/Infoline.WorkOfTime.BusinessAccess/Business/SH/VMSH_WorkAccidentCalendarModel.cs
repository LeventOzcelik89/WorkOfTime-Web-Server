﻿using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSH_WorkAccidentCalendarModel : VWSH_WorkAccidentCalendar
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMSH_WorkAccidentCalendarModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSH_WorkAccidentCalendarById(this.id);
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
            var data = db.GetVWSH_WorkAccidentCalendarById(this.id);
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

            var dbresult = db.InsertSH_WorkAccidentCalendar(new SH_WorkAccidentCalendar().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Etkinliği Oluşturma İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kaza ve Olay Etkinliği Oluşturma İşlemi Başarıyla Gerçekleşti."
                };
            }
        }
        private ResultStatus Update()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSH_WorkAccidentCalendar(new SH_WorkAccidentCalendar().B_EntityDataCopyForMaterial(this), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Etkinliği Güncelleme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kaza ve Olay Etkinliği Güncelleme İşlemi Başarıyla Gerçekleşti."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();


            var item = db.GetSH_WorkAccidentCalendarById(this.id);
            if (item == null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Etkinliği Zaten Silinmiş."
                };
            }


            var dbresult = db.DeleteSH_WorkAccidentCalendar(item, trans);
            if (item.companyPersonCalendarId.HasValue)
            {
                dbresult &= db.DeleteINV_CompanyPersonCalendar(new INV_CompanyPersonCalendar { id = item.companyPersonCalendarId.Value }, trans);
                var calenderPeople = db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(item.companyPersonCalendarId.Value);
                dbresult &= db.BulkDeleteINV_CompanyPersonCalendarPersons(calenderPeople, trans);
            }


            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Kaza ve Olay Etkinliği Silme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Kaza ve Olay Etkinliği Silme İşlemi Başarıyla Gerçekleştirildi."
                };
            }
        }
       
    }

}