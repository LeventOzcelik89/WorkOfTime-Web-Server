using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    public class VMPRD_CompanyBasedPriceModel:VWPRD_CompanyBasedPrice
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMPRD_CompanyBasedPriceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPriceById(this.id);
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
            var data = db.GetVWPRD_CompanyBasedPriceById(this.id);
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
            var dbresult = db.InsertPRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Müşteri Bazlı fiyatlandırma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Müşteri Bazlı fiyatlandırma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdatePRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Müşteri Bazlı fiyatlandırma güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Müşteri Bazlı fiyatlandırma güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeletePRD_CompanyBasedPrice(new PRD_CompanyBasedPrice { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Müşteri Bazlı fiyatlandırma işlemi silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Müşteri Bazlı fiyatlandırma işlemi silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }   
    }
}
