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
		public List<VMPRD_TransactionItemList> items { get; set; } = new List<VMPRD_TransactionItemList>() { };
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		private List<VWPRD_Product> products { get; set; }
		private List<VWPRD_Inventory> inventories { get; set; }
		public Guid? inputId { get; set; }
		public Guid? inputCompanyId { get; set; }
		public Guid? outputCompanyId { get; set; }
		public string outputTable { get; set; }
		public string inputTable { get; set; }
		public short? type { get; set; }


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
					this.items = db.GetVWPRD_TransactionItemByTransactionIds(distributionPlanRelation.Where(v => v.transactionId.HasValue).Select(c => c.transactionId.Value).ToArray()).OrderBy(a => a.created).ToList().Select(a => new VMPRD_TransactionItemList().B_EntityDataCopyForMaterial(a)).ToList();
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
				this.items.Add(new VMPRD_TransactionItemList { });
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
			var productids = this.items.Select(a => a.productId.Value).ToArray();
			var serials = this.items.Where(a => a.serialCodes != null).SelectMany(a => a.serialCodes.Split(',').Select(c => c.ToLower())).ToArray();
			this.products = db.GetVWPRD_ProductByIds(productids).ToList();
			this.inventories = db.GetVWPRD_InventoryBySerialCodesAndIds(productids, serials).ToList();

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
				//rs = this.Update(distributionPlan);
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
			var errorList = new List<string>();
			var outputInfo = db.GetVWCMP_StorageById(this.outputId.Value);
			var outputId_Title = outputInfo.fullName;
			this.outputId = outputInfo.id;


			var DBResult = new ResultStatus { result = true };
			var PRDTransactions = new List<PRD_Transaction>();
			var PRDDistributionPlan = new PRD_DistributionPlan()
			{
				id = Guid.NewGuid(),
				created = DateTime.Now,
				createdby = this.createdby,
				code = this.code,
				date = this.date,
				outputId = this.outputId,
				description = this.description,
				status = (int)EnumPRD_DistributionPlanStatus.Tamamlandi
			};
			var PRDDistributionPlanRelations = new List<PRD_DistributionPlanRelation>();
			var PRDTransactionItems = new List<PRD_TransactionItem>();
			var PRDInventories = new List<PRD_Inventory>();
			var PRDInventoryActions = new List<PRD_InventoryAction>();
			this.outputId = outputInfo.id;
			this.outputCompanyId = outputInfo.companyId;
			this.type = (int)EnumPRD_TransactionType.GidenIrsaliye;
			this.outputTable = "CMP_Storage";
			this.inputTable = "CMP_Storage";

			if (this.items.Count() > 0 && this.items.Where(a => a.productId.HasValue).Count() > 0)
			{

				var itemsGroup = this.items.GroupBy(a => a.inputId.Value).ToList();
				var productUnits = db.GetPRD_ProductUnitByProductIds(this.items.Select(b => b.productId.Value).ToArray());

				for (int i = 0; i < itemsGroup.Count(); i++)
				{
					var items = this.items.Where(a => a.inputId == itemsGroup[i].Key).ToList();

					foreach (var item in items)
					{
						this.inputId = item.inputId;
						this.inputCompanyId = item.inputCompanyId;

						var product = this.products.Where(a => a.id == item.productId).FirstOrDefault();
						if (product == null) continue;
						if (productUnits == null) continue;

						var productUnit = productUnits.Where(a => a.unitId == item.alternativeUnitId && a.productId == item.productId).FirstOrDefault();
						var defaultUnit = productUnits.Where(a => a.productId == item.productId && a.isDefault == (int)EnumPRD_ProductUnitIsDefault.Evet).FirstOrDefault();

						if (productUnit == null || defaultUnit == null) continue;

						var transItem = new PRD_TransactionItem
						{
							created = new DateTime(this.created.Value.Ticks).AddMilliseconds(i),
							createdby = this.createdby,
							productId = item.productId,
							transactionId = this.id,
							unitId = defaultUnit.unitId,
							quantity = (defaultUnit.quantity / productUnit.quantity) * item.alternativeQuantity,
							alternativeUnitId = item.alternativeUnitId,
							alternativeQuantity = item.alternativeQuantity,
							unitPrice = item.unitPrice,
							serialCodes = item.serialCodes,
						};
						//Sadece Stoklar işlendi ise envanterler yaratılır aksi takdirde yaratılmaz
						if (this.status == (int)EnumPRD_TransactionStatus.islendi && product.stockType == (int)EnumPRD_ProductStockType.SeriNoluTakip)
						{
							//errorList.AddRange(this.Validator(item));
							foreach (var serialcode in (item.serialCodes ?? "").Split(','))
							{
								var winventory = this.inventories.Where(a => a.productId == item.productId && a.serialcode == serialcode).FirstOrDefault();
								var inventory = winventory != null ? new PRD_Inventory().B_EntityDataCopyForMaterial(winventory) : new PRD_Inventory
								{
									id = inventoryId.HasValue ? inventoryId.Value : Guid.NewGuid(),
									created = DateTime.Now,
									createdby = this.createdby,
									productId = item.productId,
									code = BusinessExtensions.B_GetIdCode(),
									serialcode = serialcode,
									type = inventoryId.HasValue ? (Int16)EnumPRD_InventoryType.Sokum : (Int16)EnumPRD_InventoryType.Diger
								};
								if (winventory == null)
								{
									PRDInventories.Add(inventory);
								}
								PRDInventoryActions.Add(new PRD_InventoryAction
								{
									createdby = this.createdby,
									created = DateTime.Now,
									inventoryId = inventory.id,
									transactionId = this.id,
									transactionItemId = transItem.id,
									dataId = outputId,
									dataTable = "CMP_Storage",
									dataCompanyId = outputInfo.companyId,
									location = outputInfo.location,
									type = (int)EnumPRD_TransactionType.GelenIrsaliye
								});
								PRDInventoryActions.Add(new PRD_InventoryAction
								{
									createdby = this.createdby,
									created = DateTime.Now.AddSeconds(1),
									inventoryId = inventory.id,
									transactionId = this.id,
									transactionItemId = transItem.id,
									dataId = item.inputId,
									dataTable = "CMP_Storage",
									dataCompanyId = item.inputCompanyId,
									//location = inputInfo.Location,
									type = (int)EnumPRD_TransactionType.GelenIrsaliye
								});
							}
						}
						PRDTransactionItems.Add(transItem);
					}


					PRDTransactions.Add(new PRD_Transaction().B_EntityDataCopyForMaterial(this));
					PRDDistributionPlanRelations.Add(new PRD_DistributionPlanRelation
					{
						id = Guid.NewGuid(),
						created = DateTime.Now,
						createdby = this.createdby,
						distributionPlanId = PRDDistributionPlan.id,
						transactionId = this.id
					});
					this.id = Guid.NewGuid();
				}

			}

			if (errorList.Count() > 0)
			{
				DBResult.result = false;
				DBResult.message = "<ul style='margin:10px 0 0 0;padding:0;'>" + string.Join("", errorList.Select(a => "<li>" + a + "</li>")) + "</ul>";
				return DBResult;
			}
			
			DBResult &= db.BulkInsertPRD_Transaction(PRDTransactions, this.trans);
			DBResult &= db.InsertPRD_DistributionPlan(PRDDistributionPlan, this.trans);
			DBResult &= db.BulkInsertPRD_DistributionPlanRelation(PRDDistributionPlanRelations, this.trans);
			DBResult &= db.BulkInsertPRD_TransactionItem(PRDTransactionItems, this.trans);
			DBResult &= db.BulkInsertPRD_Inventory(PRDInventories, this.trans);
			DBResult &= db.BulkInsertPRD_InventoryAction(PRDInventoryActions, this.trans);

			return DBResult;
		}

		//}
		//private ResultStatus Update(PRD_DistributionPlan distributionPlan)
		//{

		//}

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

		//private List<string> Validator(VWPRD_TransactionItem item)
		//{
		//	var errorList = new List<string>();
		//	var product = this.products.Where(a => a.id == item.productId).FirstOrDefault();
		//	var type = (EnumPRD_TransactionType)this.type;
		//	var serialCodes = (item.serialCodes ?? "").Split(',');
		//	if (serialCodes.Count(a => a != "" && a != null) != item.quantity)
		//	{
		//		errorList.Add(string.Format("{0} ürünü için girilen serinumarası miktarı {1} ile girilen miktar {2} uyuşmamaktadır.", product.fullName, serialCodes.Count(), item.quantity));
		//	}
		//	var maintanceInventory = new List<string>();
		//	var unvalidInventory = new List<string>();
		//	var existInventory = new List<string>();
		//	var unkownInventory = new List<string>();
		//	foreach (var serialcode in serialCodes)
		//	{
		//		var winventory = this.inventories.Where(a => a.productId == item.productId && a.serialcode == serialcode).FirstOrDefault();
		//		//Kontroller Yapılacak duruma göre
		//		if (winventory != null && winventory.firstActionType == (int)EnumPRD_InventoryActionType.BakimEnvanteri)
		//		{
		//			maintanceInventory.Add(winventory.serialcode);
		//		}
		//		switch (type)
		//		{
		//			case EnumPRD_TransactionType.GelenIrsaliye:
		//			case EnumPRD_TransactionType.Kiralama:
		//				if (winventory != null && winventory.lastActionDataCompanyId != this.outputCompanyId) unvalidInventory.Add(serialcode);
		//				break;
		//			case EnumPRD_TransactionType.ZimmetAlma:
		//				if (winventory == null) unkownInventory.Add(serialcode);
		//				if (winventory != null && winventory.lastActionDataId != this.outputId) unvalidInventory.Add(serialcode);
		//				break;
		//			case EnumPRD_TransactionType.AcilisFisi:
		//			case EnumPRD_TransactionType.UretimFisi:
		//			case EnumPRD_TransactionType.SayimFazlasi:
		//				if (winventory != null && winventory.lastActionDataId != this.outputId) existInventory.Add(serialcode);
		//				break;
		//			case EnumPRD_TransactionType.GidenIrsaliye:
		//			case EnumPRD_TransactionType.ZimmetVerme:
		//			case EnumPRD_TransactionType.KiralayaVerme:
		//			case EnumPRD_TransactionType.SarfFisi:
		//			case EnumPRD_TransactionType.FireFisi:
		//			case EnumPRD_TransactionType.SayimEksigi:
		//			case EnumPRD_TransactionType.Transfer:
		//				if (winventory == null) unvalidInventory.Add(serialcode);
		//				if (winventory != null && winventory.lastActionDataId != this.outputId) unvalidInventory.Add(serialcode);
		//				break;
		//			default:
		//				break;
		//		}
		//	}
		//	if (existInventory.Count() > 0)
		//	{
		//		errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları  <strong>{2}</strong> zaten farklı carilerde kayıtlı.", product.fullName, string.Join(",", existInventory), this.outputId_Title));
		//	}
		//	if (unvalidInventory.Count() > 0)
		//	{
		//		errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları  <strong>{2}</strong> çıkış carisinde bulunmamaktadır.", product.fullName, string.Join(",", unvalidInventory), this.outputId_Title));
		//	}
		//	if (unkownInventory.Count() > 0)
		//	{
		//		errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları stoklarda bulunmamaktadır. Sadece stoklarda bulunan serinumaraları ile işlem yapabilirsiniz.", product.fullName, string.Join(",", unkownInventory)));
		//	}
		//	if (maintanceInventory.Count() > 0)
		//	{
		//		errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları bakım envanteri olarak kayıtlıdır.Bu envanterler üzerinde işlem yapamazsınız.", product.fullName, string.Join(",", maintanceInventory)));
		//	}
		//	return errorList;
		//}

	}

	public class VMPRD_TransactionItemList : VWPRD_TransactionItem
	{
		public string brand_Title { get; set; }
		public string categoryId_Title { get; set; }
		public string description { get; set; }
		public short? stockType { get; set; }
		public string currencyTitle { get; set; }
		public Guid? inputCompanyId { get; set; }
		public Guid? inputId { get; set; }

	}

}