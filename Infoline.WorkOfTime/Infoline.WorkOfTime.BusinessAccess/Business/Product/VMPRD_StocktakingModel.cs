using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class VMPRD_StocktakingModel : VWPRD_Stocktaking
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPRD_StocktakingItem[] items { get; set; }
		public VWPRD_StockTakingItemModel[] stockTakingSummaries { get; set; }
		public VMPRD_StocktakingModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StocktakingById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
                this.items = db.GetVWPRD_StocktakingItemByStocktakingId(this.id);
                stockTakingSummaries = this.items.GroupBy(a=>a.).Select(a => new VWPRD_StockTakingItemModel
                {


                });
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWPRD_StocktakingById(this.id);
            var res = new ResultStatus { result = true };
            var validation = Validator();
            if (this.code == null)
            {
                this.code = BusinessExtensions.B_GetIdCode();
            }
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
            this.code = BusinessExtensions.B_GetIdCode();
            var dbresult = db.InsertPRD_Stocktaking(this.B_ConvertType<PRD_Stocktaking>(), this.trans);
            dbresult &= db.BulkInsertPRD_StocktakingItem(this.items.Select(a => new PRD_StocktakingItem
            {
                id = Guid.NewGuid(),
                created = this.created,
                createdby = this.createdby,
                stocktakingId = this.id,
                productId = a.productId,
                serialNumber = a.serialNumber,
                quantity = a.quantity,
                unitId = a.unitId,
            }), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Sayım oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Sayım oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdatePRD_Stocktaking(this.B_ConvertType<PRD_Stocktaking>(), false, this.trans);

            var items = db.GetVWPRD_StocktakingItemByStocktakingId(this.id);
            dbresult &= db.BulkDeletePRD_StocktakingItem(items.Select(a => new PRD_StocktakingItem
            {
                id = a.id,
                created = a.created,
                createdby = a.createdby,
                stocktakingId = a.stocktakingId,
                productId = a.productId,
                serialNumber = a.serialNumber,
                quantity = a.quantity,
                unitId = a.unitId,
            }), this.trans);

            dbresult &= db.BulkInsertPRD_StocktakingItem(this.items.Select(a => new PRD_StocktakingItem
            {
                id = Guid.NewGuid(),
                created = a.created,
                createdby = a.createdby,
                stocktakingId = this.id,
                productId = a.productId,
                serialNumber = a.serialNumber,
                quantity = a.quantity,
                unitId = a.unitId,
            }), this.trans);

            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Sayım güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Sayım güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeletePRD_Stocktaking(this.id, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Müşteri silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Müşteri silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        public SummaryHeadersStocktaking GetPageInfo(Guid userId)
        {
            this.Load();
            return db.GetVWPRD_StocktakingPageInfo(userId);
        }


    }

    public class VWPRD_StockTakingItemModel : VWPRD_StocktakingItem
	{
		public int storageQuantity { get; set; }
	}
}
