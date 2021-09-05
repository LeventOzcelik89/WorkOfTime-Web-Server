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

		public VMPRD_ProductionModel Load()
		{
			db = db ?? new WorkOfTimeDatabase();
			var production = db.GetVWPRD_ProductionById(this.id);

			if (production != null)
			{
				this.B_EntityDataCopyForMaterial(production, true);
				productionUsers = db.GetVWPRD_ProductionUserByProductionId(this.id).ToList();
				productionOperations = db.GetVWPRD_ProductionOperationByProductionId(this.id).ToList();
				productionStages = db.GetVWPRD_ProductionStagesByProductionId(this.id).ToList();
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
						status = (int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
						description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcıları üretime dahil oldu."
					});
				}
			}


			var productionSchemaStages = db.GetVWPRD_ProductionSchemaStageByStageSchemaId(this.productionSchemaId.Value);

			productionStages.AddRange(productionSchemaStages.Select(x => new VWPRD_ProductionStage
			{
				id = Guid.NewGuid(),
				createdby = this.createdby,
				code = x.code,
				name = x.name,
				productionId = this.id,
				stageNumber = x.stageNum
			}));

			var rs = new ResultStatus { result = true };
			rs = db.InsertPRD_Production(new PRD_Production().B_EntityDataCopyForMaterial(this), this.trans);
			rs &= db.BulkInsertPRD_ProductionOperation(productionOperations.Select(a => new PRD_ProductionOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
			rs &= db.BulkInsertPRD_ProductionUser(productionUsers.Select(a => new PRD_ProductionUser().B_EntityDataCopyForMaterial(a, true)), this.trans);
			rs &= db.BulkInsertPRD_ProductionStage(productionStages.Select(a => new PRD_ProductionStage().B_EntityDataCopyForMaterial(a, true)), this.trans);


			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = "xxx Kodlu Üretim emri başarıyla oluşturuldu." };
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

			var production = new PRD_Production().B_EntityDataCopyForMaterial(this, true);

			var rs = db.UpdatePRD_Production(production, true, transaction);

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
		public ResultStatus Delete(Guid? userId, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var production = db.GetPRD_ProductionById(this.id);
			if (production == null)
			{
				return new ResultStatus { result = false, message = "Üretim emri silinmiş." };
			}

			var _productionOperations = db.GetPRD_ProductionOperationByProductionId(production.id);
			var _productionUsers = db.GetPRD_ProductionUsersByProductionId(production.id);
			var _productionStages = db.GetPRD_ProductionStageByProductionId(production.id);
			var _productionProduct = db.GetPRD_ProductionProductByProductionId(production.id);

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
					message = "Üretim silme işlemi başarısız."
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


		public void InsertProductionProducts(string[] materiels, Guid? productionId, Guid? userId)
		{
			if (materiels.Count() > 0)
			{
				var db = new WorkOfTimeDatabase();
				var productionProducts = new List<PRD_ProductionProduct>();

				foreach (var material in materiels)
				{
					var materialSplitData = material.Split('!');
					productionProducts.Add(new PRD_ProductionProduct
					{
						id = Guid.NewGuid(),
						createdby = userId,
						productId = new Guid(materialSplitData[0]),
						price =  Convert.ToDouble(materialSplitData[1]),
						materialId = new Guid(materialSplitData[2]),
						quantity = 0,
						productionId = productionId,
						type = (int)EnumPRD_ProductionProductsType.RecetedenGelen

					});
				}

				db.BulkInsertPRD_ProductionProduct(productionProducts);
			}

		}

		public VWPRD_StockSummary[] ProductStocksByProductIdsAndStorageId(Guid[] productIds, Guid storageId)
		{
			var db = new WorkOfTimeDatabase();
			var stockSummary = db.GetVWPRD_StockSummaryByProductIdsAndStockId(productIds, storageId);
			return stockSummary;
		}
	}
}
