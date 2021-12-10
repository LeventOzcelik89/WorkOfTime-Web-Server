using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess.Business.SH
{
    public class VWSH_ReportModel : VWSH_Report
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWSH_ReportModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSH_ReportById(this.id);
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
            var data = db.GetVWSH_ReportById(this.id);
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
            //Validasyonlarını yap
            var dbresult = db.InsertSH_Report(new SH_Report().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıtlı Raporlarım oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıtlı Raporlarım oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSH_Report(new SH_Report().B_EntityDataCopyForMaterial(this), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıtlı Raporlarım güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıtlı Raporlarım güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSH_Report(new SH_Report { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıtlı Raporlarım silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıtlı Raporlarım silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public static ResultStatus Import()
        {
            return null;
        }

        public static PageModel<VWSH_Report> GetPageModel(PageSecurity pageSecurity, string pageName)
        {

            var model = new PageModel<VWSH_Report> { Title = pageName };

            if (pageName == "Index")
            {
                model.Name = "Kayıtlı Raporlarım";
                model.SetGridFilter(a => a.createdby == pageSecurity.user.id);
            }

            return model;

        }

        public static SimpleQuery UpdateDataSourceFilter(SimpleQuery query, PageSecurity userStatus)
        {
            //BEXP filter = null;
            //filter = new BEXP
            //{
            //    Operand1 = new BEXP
            //    {
            //        Operand1 = (COL)"status",
            //        Operator = BinaryOperator.Equal,
            //        Operand2 = (VAL)1
            //    },
            //    Operator = BinaryOperator.Or,
            //    Operand2 = new BEXP
            //    {
            //        Operand1 = (COL)"status",
            //        Operator = BinaryOperator.Equal,
            //        Operand2 = (VAL)3
            //    }
            //};
            //query.Filter &= filter;
            return query;
        }
    }
}
