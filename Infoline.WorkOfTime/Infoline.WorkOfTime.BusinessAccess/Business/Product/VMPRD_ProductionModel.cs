using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class ProductionProductModel : VWPRD_Production
	{ }

	public class VMPRD_ProductionModel : VWPRD_Production
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		public VWCMP_Company productionCompany { get; set; }
		public VWCMP_Storage prodictionStorage { get; set; }
		public VWCMP_Company centerCompany { get; set; }
		public VWCMP_Storage centerStorage { get; set; }
		public List<Guid> assignableUsers { get; set; }
		public List<VWPRD_ProductionUser> productionUsers { get; set; } = new List<VWPRD_ProductionUser>();
		public List<VWPRD_ProductionOperation> productionOperations { get; set; } = new List<VWPRD_ProductionOperation>();
		public List<VWPRD_ProductionStage> productionStages { get; set; } = new List<VWPRD_ProductionStage>();
		public List<VWPRD_ProductMateriel> productMaterials { get; set; } = new List<VWPRD_ProductMateriel>();
		public List<VWPRD_ProductionProduct> productionProducts { get; set; } = new List<VWPRD_ProductionProduct>();
		public Guid? productionSchemaId { get; set; }
		public string materialString { get; set; }

		public VMPRD_ProductionModel Load()
		{
			db = db ?? new WorkOfTimeDatabase();
			var production = db.GetVWPRD_ProductionById(this.id);

			if (production != null)
			{
				this.B_EntityDataCopyForMaterial(production, true);
				productionUsers = db.GetVWPRD_ProductionUserByProductionId(this.id).ToList();
				productionProducts = db.GetVWPRD_ProductionProductByProductId(this.id).ToList();
				productionOperations = db.GetVWPRD_ProductionOperationByProductionId(this.id).ToList();
				productionStages = db.GetVWPRD_ProductionStagesByProductionId(this.id).ToList();
				assignableUsers = productionUsers.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();
				productionSchemaId = productionStages.Count() > 0 ? productionStages.Where(a => a.productionSchemaId.HasValue).Select(x => x.productionSchemaId.Value).FirstOrDefault() : Guid.NewGuid();
			}
			this.code = !String.IsNullOrEmpty(this.code) ? this.code : BusinessExtensions.B_GetIdCode();
			return this;
		}

		public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var production = db.GetVWPRD_ProductionById(this.id);
			var rs = new ResultStatus { result = true };

			if (production == null)
			{
				this.createdby = userId;
				this.created = DateTime.Now;
				rs = Insert(userId, trans);
			}
			else
			{
				this.changedby = userId;
				this.changed = DateTime.Now;
				rs = Update(userId, trans);
			}

			if (rs.result)
			{
				if (request != null)
				{
					new FileUploadSave(request, this.id).SaveAs();
				}
			}

			return rs;
		}

		private ResultStatus Insert(Guid? userId, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();
			var production = new PRD_Production().B_EntityDataCopyForMaterial(this, true);
			var validatorResult = Validator();
			if (!validatorResult.result) return validatorResult;

			var createOperation = new VWPRD_ProductionOperation
			{
				id = Guid.NewGuid(),
				createdby = userId,
				created = DateTime.Now,
				productionId = this.id,
				dataId = this.id,
				dataTable = "PRD_Production",
				status = (int)EnumPRD_ProductionOperationStatus.UretimEmriVerildi,
				description = this.description,
			};

			productionOperations.Add(createOperation);

			if (assignableUsers != null && assignableUsers.Count() > 0)
			{
				var users = db.GetVWSH_UserByIds(this.assignableUsers.ToArray());

				if (users.Count() > 0)
				{
					productionUsers.AddRange(users.Select(x => new VWPRD_ProductionUser
					{
						id = Guid.NewGuid(),
						createdby = this.createdby,
						productionId = this.id,
						userId = x.id
					}));

					productionOperations.Add(new VWPRD_ProductionOperation
					{
						id = Guid.NewGuid(),
						createdby = userId,
						created = DateTime.Now.AddSeconds(1),
						productionId = this.id,
						status = (int)EnumPRD_ProductionOperationStatus.PersonelAtamasiYapildi,
						description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcıları üretime dahil oldu."
					});
				}
			}


			var productionSchemaStages = db.GetVWPRD_ProductionSchemaStageByStageSchemaId(this.productionSchemaId.Value);
			var rs = new ResultStatus { result = true };

			productionStages.AddRange(productionSchemaStages.Select(x => new VWPRD_ProductionStage
			{
				id = Guid.NewGuid(),
				createdby = this.createdby,
				code = x.code,
				name = x.name,
				productionId = this.id,
				stageNumber = x.stageNum,
				productionSchemaId = this.productionSchemaId
			}));

			if (!string.IsNullOrEmpty(this.materialString))
			{
				var materials = Infoline.Helper.Json.Deserialize<List<VWPRD_ProductMateriel>>(this.materialString);
				if (materials.Count() > 0)
				{
					var productionProducts = new List<PRD_ProductionProduct>();

					productionProducts.AddRange(materials.Select(x => new PRD_ProductionProduct
					{
						id = Guid.NewGuid(),
						createdby = userId,
						productId = x.productId,
						price = x.price,
						materialId = x.materialId,
						quantity = x.quantity,
						productionId = this.id,
						totalQuantity = x.totalQuantity,
						type = (int)EnumPRD_ProductionProductsType.RecetedenGelen
					}));

					rs &= db.BulkInsertPRD_ProductionProduct(productionProducts);
				}
			}

			rs = db.InsertPRD_Production(new PRD_Production().B_EntityDataCopyForMaterial(this), this.trans);
			rs &= db.BulkInsertPRD_ProductionOperation(productionOperations.Select(a => new PRD_ProductionOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
			rs &= db.BulkInsertPRD_ProductionUser(productionUsers.Select(a => new PRD_ProductionUser().B_EntityDataCopyForMaterial(a, true)), this.trans);
			rs &= db.BulkInsertPRD_ProductionStage(productionStages.Select(a => new PRD_ProductionStage().B_EntityDataCopyForMaterial(a, true)), this.trans);


			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = this.code + " kodlu üretim emri başarıyla oluşturuldu." };
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus { result = false, message = "Üretim emri oluşturma işlemi başarısız." };
			}
		}

		private ResultStatus Update(Guid? userId, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();
			this.assignableUsers = this.assignableUsers ?? new List<Guid>();
			var rs = new ResultStatus { result = true };

			var insertUsers = new List<PRD_ProductionUser>();
			var users = new List<VWSH_User>();
			var currentUserDatas = db.GetPRD_ProductionUsersByProductionId(this.id);
			var deletedUsers = currentUserDatas.Where(a => !this.assignableUsers.Contains(a.userId.Value));
			var newInsertUsers = this.assignableUsers.Where(a => !currentUserDatas.Select(c => c.userId).Contains(a)).ToArray();

			if (newInsertUsers.Count() > 0)
			{
				users = db.GetVWSH_UserByIds(newInsertUsers).ToList();
				var currentUsers = db.GetPRD_ProductionUsersByProductionId(this.id);
				rs &= db.BulkDeletePRD_ProductionUser(currentUsers, trans);
			}

			if (users.Count() > 0)
			{
				var now = DateTime.Now;
				productionOperations.Add(new VWPRD_ProductionOperation
				{
					createdby = userId,
					created = DateTime.Now,
					productionId = this.id,
					status = (int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
					description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına atama yapıldı."
				});

				insertUsers = assignableUsers.Select(a => new PRD_ProductionUser
				{
					createdby = userId,
					created = (DateTime.Now),
					userId = a,
					productionId = this.id,
				}).ToList();
			}

			rs &= db.BulkInsertPRD_ProductionOperation(productionOperations.Select(x => new PRD_ProductionOperation().B_EntityDataCopyForMaterial(x, true)), trans);
			rs &= db.BulkDeletePRD_ProductionUser(deletedUsers, trans);
			rs &= db.BulkInsertPRD_ProductionUser(insertUsers, trans);

			var production = new PRD_Production().B_EntityDataCopyForMaterial(this, true);
			rs &= db.UpdatePRD_Production(production, true, transaction);

			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = "Üretim emri başarıyla güncellendi." };
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus { result = false, message = "Ürün emri güncelleme işlemi başarısız." };
			}
		}

		public ResultStatus Delete(Guid? userId, DbTransaction _trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			this.trans = _trans ?? this.db.BeginTransaction();
			var production = db.GetPRD_ProductionById(this.id);
			if (production == null)
			{
				return new ResultStatus { result = false, message = "Üretim emri silinmiş." };
			}

			//	THİS.LOAD()
			var _productionOperations = db.GetPRD_ProductionOperationByProductionId(production.id);
			var _productionUsers = db.GetPRD_ProductionUsersByProductionId(production.id);
			var _productionStages = db.GetPRD_ProductionStageByProductionId(production.id);
			var _productionProduct = db.GetPRD_ProductionProductByProductionId(production.id);

			//	üretim gerçekleştirildiyse silinemez !...
			//	50 üretildi gridinde sil olacak.

			var rs = db.BulkDeletePRD_ProductionUser(_productionUsers, trans);
			rs &= db.BulkDeletePRD_ProductionOperation(_productionOperations, trans);
			rs &= db.BulkDeletePRD_ProductionStage(_productionStages);
			rs &= db.BulkDeletePRD_ProductionProduct(_productionProduct);
			rs &= db.DeletePRD_Production(production, trans);


			if (rs.result == true)
			{
				if (trans == null) trans.Commit();
				return new ResultStatus
				{
					result = true,
					message = "Üretim emri silme başarılı."
				};
			}
			else
			{
				if (trans == null) trans.Rollback();
				return new ResultStatus
				{
					result = false,
					message = rs.message
				};
			}
		}


		public ResultStatus Validator()
		{
			if (!this.productionSchemaId.HasValue)
			{
				return new ResultStatus { message = "Üretim şeması zorunludur." };
			}
			if (!this.productionCompanyId.HasValue)
			{
				return new ResultStatus { result = false, message = "Üretimin yapılacağı şirket zorunludur." };
			}

			if (!this.productionStorageId.HasValue)
			{
				return new ResultStatus { result = false, message = "Ürünlerin yerinin deposu zorunludur." };
			}

			if (!this.centerCompanyId.HasValue)
			{
				return new ResultStatus { result = false, message = "Malzemelerin alınacağı şirket zorunludur." };
			}

			if (!this.centerStorageId.HasValue)
			{
				return new ResultStatus { result = false, message = "Malzemelerin alınacağı depo zorunludur." };
			}

			if (!this.productId.HasValue)
			{
				return new ResultStatus { result = false, message = "Üretim emri verilecek ürün zorunludur." };
			}

			if (!this.quantity.HasValue)
			{
				return new ResultStatus { result = false, message = "Üretim adedi zorunludur." };
			}

			//if (this.productMaterials.Count() <= 0)
			//{
			//	return new ResultStatus { result = false, message = "Ürünün reçetesi bulunmamaktadır." };
			//}

			return new ResultStatus { result = true };
		}


		public void InsertProductionProducts(VWPRD_ProductMateriel[] materiels, Guid? productionId, Guid? userId)
		{
			if (materiels.Count() <= 0)
			{
				return;
			}

			var db = new WorkOfTimeDatabase();
			var productionProducts = new List<PRD_ProductionProduct>();

			productionProducts.AddRange(materiels.Select(x => new PRD_ProductionProduct
			{
				id = Guid.NewGuid(),
				createdby = userId,
				productId = x.productId,
				price = x.price,
				materialId = x.materialId,
				quantity = x.quantity,
				productionId = productionId,
				totalQuantity = x.totalQuantity,
				type = (int)EnumPRD_ProductionProductsType.RecetedenGelen
			}));

			db.BulkInsertPRD_ProductionProduct(productionProducts);
		}

		public VWPRD_StockSummary[] ProductStocksByProductIdsAndStorageId(Guid[] productIds, Guid storageId)
		{
			var db = new WorkOfTimeDatabase();
			var stockSummary = db.GetVWPRD_StockSummaryByProductIdsAndStockId(productIds, storageId);
			return stockSummary;
		}

		public VWPRD_ProductionProduct[] GetProductionProductAndTransaction(Guid productionId)
		{
			var db = new WorkOfTimeDatabase();
			var productionProductList = new List<VWPRD_ProductionProduct>();
			var newProductionProductList = new List<VWPRD_ProductionProduct>();

			var data = new VMPRD_ProductionModel { id = productionId }.Load();
			var productionOperations = data.productionOperations.Where(x => x.status == (int)EnumPRD_ProductionOperationStatus.HarcamaBildirildi).ToList();

			if (productionOperations.Count() > 0)
			{
				var transactionItems = db.GetVWPRD_TransactionItemByTransactionIds(productionOperations.Where(a => a.dataId.HasValue).Select(x => x.dataId.Value).ToArray());

				foreach (var productionItem in data.productionProducts)
				{
					var transactionItem = transactionItems.Where(x => x.productId == productionItem.materialId).FirstOrDefault();

					if (transactionItem != null)
					{
						var product = data.productionProducts.Where(x => x.materialId == transactionItem.productId).FirstOrDefault();
						if (product != null)
						{
							if (productionProductList.Where(x => x.materialId == product.materialId).Count() > 0)
							{
								continue;
							}

							var transaction = transactionItems.Where(x => x.productId == product.materialId).Select(x => x.quantity);
							var amountSpent = transaction.Sum();
							product.amountSpent = amountSpent;
							productionProductList.Add(product);
						}
						else
						{
							newProductionProductList.Add(new VWPRD_ProductionProduct
							{
								id = Guid.NewGuid(),
								price = transactionItem.unitPrice,
								productionId = productionId,
								materialId = transactionItem.productId,
								serialCodes = transactionItem.serialCodes,
								type = (int)EnumPRD_ProductionProductsType.SonradanEklenen,
								amountSpent = transactionItem.quantity,
								totalQuantity = 0,
								quantity = 0,
								productId = this.productId
							});

						}
					}
					else
					{
						productionProductList.Add(productionItem);
					}

				}
				db.BulkUpdatePRD_ProductionProduct(productionProductList.Select(a => new PRD_ProductionProduct().B_EntityDataCopyForMaterial(a, true)));
				db.BulkInsertPRD_ProductionProduct(newProductionProductList.Select(a => new PRD_ProductionProduct().B_EntityDataCopyForMaterial(a, true)));
			}
			else
			{
				return data.productionProducts.ToArray();
			}

			return productionProductList.ToArray();
		}

	}
}
