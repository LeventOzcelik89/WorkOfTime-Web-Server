using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    public class VMPRD_ProductBasedPriceModel: VWPRD_CompanyBasedPriceDetail
    {

        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPRD_CompanyBasedPriceDetail Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetW(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            else
            {
                data = db.GetVWEMP_EmployeeChangesLastByEmployeeId(this.employeeId);
                if (data != null)
                {
                    this.B_EntityDataCopyForMaterial(data, true);
                }
                this.statusChangeId = null;
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new InfolineHRDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWEMP_EmployeeChangesById(this.id);
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
                if (request != null)
                {
                    new FileUploadWebProvider(request, this.id).SaveAs(userId);
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
            db = db ?? new InfolineHRDatabase();
            var res = new ResultStatus { result = true };
            return res;
        }
        private ResultStatus Insert()
        {
            db = db ?? new InfolineHRDatabase();
            //Validasyonlarını yap
            var dbresult = db.InsertEMP_EmployeeChanges(new EMP_EmployeeChanges().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateEMP_EmployeeChanges(new EMP_EmployeeChanges().B_EntityDataCopyForMaterial(this), false, this.trans);
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
            db = db ?? new InfolineHRDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteEMP_EmployeeChanges(new EMP_EmployeeChanges { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public static ResultStatus Import()
        {
            return null;
        }
        public static PageModel GetPageModel()
        {
            var pageModel = new PageModel { Title = "Çalışan Durum Değişiklikleri" };
            return pageModel;
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
