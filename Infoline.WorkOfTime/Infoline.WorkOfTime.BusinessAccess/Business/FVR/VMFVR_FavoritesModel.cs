using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VWFVR_FavoritesModel : VWFVR_Favorites
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWFVR_FavoritesModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWFVR_FavoritesById(this.id);
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
            var data = db.GetVWFVR_FavoritesById(this.id);
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
            var dbresult = db.InsertVWFVR_Favorites(new VWFVR_Favorites
            {
                userId = createdby
            }.B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Favori oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Favori oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateVWFVR_Favorites(new VWFVR_Favorites().B_EntityDataCopyForMaterial(this), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Favori güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Favori güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteVWFVR_Favorites(new VWFVR_Favorites { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Favori silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Favori silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public static ResultStatus Import()
        {
            return null;
        }
        public static PageModel GetPageModel()
        {
            var pageModel = new PageModel { Title = "Favori" };
            return pageModel;
        }
        public static SimpleQuery UpdateDataSourceFilter(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            filter = new BEXP
            {
                Operand1 = (COL)"userId",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)userStatus.user.id

            };
            query.Filter &= filter;
            return query;
        }
    }
}
