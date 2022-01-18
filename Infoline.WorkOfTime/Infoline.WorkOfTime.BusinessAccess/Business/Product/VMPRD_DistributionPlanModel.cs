using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPRD_DistributionPlanModel : VWPRD_DistributionPlan
	{
		public List<VMPRD_TransactionItems> items { get; set; } = new List<VMPRD_TransactionItems>() { };
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		private List<VWPRD_Product> products { get; set; }
		private List<VWPRD_Inventory> inventories { get; set; }
		public VMPRD_DistributionPlanModel Load()
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var distributionPlan = db.GetVWPRD_DistributionPlanById(this.id);
			if (distributionPlan != null)
			{
				this.B_EntityDataCopyForMaterial(distributionPlan, true);

				var distributionPlanRelation = db.GetPRD_DistributionPlanRelationByDistributionPlanId(distributionPlan.id);

				if (distributionPlanRelation.Count() > 0)
				{
					this.items = db.GetVWPRD_TransactionItemByTransactionIds(distributionPlanRelation.Where(v=>v.transactionId.HasValue).Select(c=>c.transactionId.Value).ToArray()).OrderBy(a => a.created).ToList().Select(a => new VMPRD_TransactionItems().B_EntityDataCopyForMaterial(a)).ToList();
				}

			}
			else
			{
				this.code = string.IsNullOrEmpty(this.code) ? BusinessExtensions.B_GetIdCode() : this.code;
				this.date = this.date ?? DateTime.Now;
			}
			
			if (this.items != null && this.items.Count() > 0)
			{
				var products = db.GetVWPRD_ProductByIds(this.items.GroupBy(a => a.productId).Where(a => a.Key != null).Select(a => a.Key.Value).ToArray());
				foreach (var item in this.items)
				{
					item.productId_Title = products.Where(x => x.id == item.productId).Select(a => a.fullName).FirstOrDefault();
					item.brand_Title = products.Where(x => x.id == item.productId).Select(a => a.brandId_Title).FirstOrDefault();
					item.categoryId_Title = products.Where(x => x.id == item.productId).Select(a => a.categoryId_Title).FirstOrDefault();
					//item.unitId_Title = products.Where(x => x.id == item.productId).Select(a => a.unitId_Title).FirstOrDefault();
					item.description = products.Where(x => x.id == item.productId).Select(a => a.description).FirstOrDefault();
					item.stockType = products.Where(x => x.id == item.productId).Select(x => x.stockType).FirstOrDefault();
					item.currencyTitle = products.Where(x => x.id == item.productId).Select(x => x.currentSellingCurrencyId_Title).FirstOrDefault();
				}
			}
			
			if (this.items.Count() == 0)
			{
				this.items.Add(new VMPRD_TransactionItems { });
			}
			return this;
		}
		public ResultStatus Save(Guid? userId, DbTransaction _trans = null, Guid? inventoryId = null)
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			this.trans = _trans ?? db.BeginTransaction();
			var distributionPlan = db.GetPRD_DistributionPlanById(this.id);
			this.items = this.items.Where(a => a.productId.HasValue && a.alternativeQuantity > 0).ToList();
			this.code = string.IsNullOrEmpty(this.code) ? BusinessExtensions.B_GetIdCode() : this.code;
			this.status = this.status ?? (int)EnumPRD_TransactionStatus.beklemede;
			this.date = this.date ?? DateTime.Now;

			var rs = new ResultStatus { result = true };
			if (distributionPlan == null)
			{
				this.created = DateTime.Now;
				this.createdby = userId;
				rs = this.Insert(inventoryId);
			}
			else
			{
				this.created = distributionPlan.created;
				this.createdby = distributionPlan.createdby;
				this.changed = DateTime.Now;
				this.changedby = userId;
				rs = this.Update(distributionPlan);
			}
			if (rs.result == true)
			{
				if (_trans == null) trans.Commit();
			}
			else
			{
				if (_trans == null) trans.Rollback();
			}
			return rs;
		}
		
		private ResultStatus Insert(Guid? inventoryId)
		{
			var DBResult = new ResultStatus { result = true };


		
		}
		private ResultStatus Update(PRD_DistributionPlan distributionPlan)
		{

		}

		public ResultStatus Delete(DbTransaction _trans = null)
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var transaction = db.GetPRD_TransactionById(this.id);
			if (transaction != null)
			{
				this.trans = _trans ?? db.BeginTransaction();
				var transactionItems = db.GetPRD_TransactionItemByTransactionId(transaction.id);
				var inventoryActions = db.GetPRD_InventoryActionByTransactionId(transaction.id);
				var rs = new ResultStatus { result = true };
				if (inventoryActions.Count() > 0)
				{
					var inventories = db.GetVWPRD_InventoryByIds(inventoryActions.GroupBy(a => a.inventoryId.Value).Select(a => a.Key).ToArray());
					var deleteinventories = inventories.Where(a => a.firstTransactionId == transaction.id).Select(a => new PRD_Inventory { id = a.id }).ToArray();
					var deleteactions = inventoryActions.Where(a => inventories.Select(c => c.lastTransactionItemId).Contains(a.transactionItemId)).ToArray();
					rs &= db.BulkDeletePRD_InventoryAction(deleteactions, trans);
					rs &= db.BulkDeletePRD_Inventory(deleteinventories, trans);
				}
				rs &= db.BulkDeletePRD_TransactionItem(transactionItems, trans);
				rs &= db.DeletePRD_Transaction(transaction, trans);
				if (rs.result == true)
				{
					if (_trans == null) trans.Commit();
					return new ResultStatus { message = "İşlem başarıyla silindi.", result = true, };
				}
				else
				{
					if (_trans == null) trans.Rollback();
					return new ResultStatus { message = "İşlem silinirken sorunlar oluştu.", result = false, };
				}
			}
			else
			{
				return new ResultStatus { message = "İşlem kaydı bulunamadı.", result = false, };
			}
		}
	}
}