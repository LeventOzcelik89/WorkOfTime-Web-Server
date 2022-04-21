using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPRD_StocktakingModel : VWPRD_Stocktaking
	{
		public VWPRD_Transaction Transaction { get; set; }
		public List<VMPRD_TransactionItems> transactionItems { get; set; } = new List<VMPRD_TransactionItems>();
		public VMPRD_TransactionItems transactionItem { get; set; }
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		public VWPRD_StocktakingItem[] items { get; set; }
		public List<VWPRD_StockTakingItemModel> stockTakingSummaries { get; set; } = new List<VWPRD_StockTakingItemModel>();
		public VWPRD_StocktakingUser[] stocktakingUsers { get; set; }
		public List<Guid> stocktakingUserIds { get; set; }
		public int Total { get; set; } = 0;
		public Guid? productionSchemaId { get; set; }
		public string inputId_Adress { get; set; }
		public string outputId_Adress { get; set; }
		public string tenderIds { get; set; }
		public int amount { get; set; }
		public double OutOfStock { get; set; }
		public string ProductSerialCodes { get; set; }
		public bool expensReport { get; set; }

		public VMPRD_StocktakingModel Load()
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var data = db.GetVWPRD_StocktakingById(this.id);
			if (data != null)
			{
				this.B_EntityDataCopyForMaterial(data, true);
				this.items = db.GetVWPRD_StocktakingItemByStocktakingId(this.id).B_RemoveGeographies();
				this.stocktakingUsers = db.GetVWPRD_StocktakingUserByStocktakingId(this.id);
				stockTakingSummaries.AddRange(this.items.Where(a => a.serialNumber == null).GroupBy(a => a.productId).Select(a => new VWPRD_StockTakingItemModel
				{
					productId = a.Select(b => b.productId).FirstOrDefault(),
					productId_Title = a.Select(b => b.productId_Title).FirstOrDefault(),
					storageQuantity = this.items.Count(b => b.storageId == a.Key),
					quantity = a.Sum(b => b.quantity),
					unitId = a.Select(b => b.unitId).FirstOrDefault(),
					unitId_Title = a.Select(b => b.unitId_Title).FirstOrDefault(),
					status = a.Select(b => b.status).FirstOrDefault(),
					serialNumber = a.Select(b => b.serialNumber).FirstOrDefault(),
					storageId = a.Select(b => b.storageId).FirstOrDefault()
				}).ToList());


				this.storageId = this.storageId.HasValue ? this.storageId.Value : Guid.NewGuid();

				var storageInventories = db.GetVWPRD_InventoryByLastActionDataId(this.storageId.Value);

				this.items = this.items.Where(c => c.serialNumber != null).ToArray();
				var inventories = this.items.GroupBy(a => a.productId).ToArray();
				foreach (var item in inventories)
				{
					var inventory = storageInventories.Where(a => a.productId == item.Key).ToArray();

					var cases = "";

					if (item.Select(a => a.quantity).FirstOrDefault() > inventory.Count())
					{
						cases = "Sayım Fazlası";
					}
					else if (item.Select(a => a.quantity).FirstOrDefault() > 0 && inventory.Count() == 0)
					{
						cases = "Envanter Farklı Bir Depoda";
					}
					else if (item.Select(a => a.quantity).FirstOrDefault() < inventory.Count())
					{
						cases = "Sayım Eksiği";
					}


					stockTakingSummaries.Add(new VWPRD_StockTakingItemModel
					{
						productId = item.Key,
						productId_Title = item.Select(a => a.productId_Title).FirstOrDefault(),
						storageQuantity = this.items.Count(b => b.productId == item.Key),
						quantity = this.items.Where(a => a.productId == item.Key).Count(),
						unitId = item.Select(a => a.unitId).FirstOrDefault(),
						unitId_Title = item.Select(a => a.unitId_Title).FirstOrDefault(),
						status = item.Select(a => a.status).FirstOrDefault(),
						serialNumber = string.Join(",", this.items.Where(a => a.productId == item.Key).Select(a => a.serialNumber).ToArray()),
						caseDescription = cases,
						storageId = item.Select(b => b.storageId).FirstOrDefault()
					});
				}
			}

			this.code = this.code ?? BusinessExtensions.B_GetIdCode();
			this.stocktakingUserIds = this.stocktakingUserIds ?? new List<Guid>();
			this.status = this.status ?? (int)EnumPRD_StocktakingStatus.SayimBasladi;

			return this;
		}
		public VMPRD_StocktakingModel LoadTransaction()
		{
			var transaction = new VMPRD_TransactionModel();
			transaction.B_EntityDataCopyForMaterial(this.Transaction);
			transaction.items.Add(transactionItem);
			this.transactionItems.Add(transactionItem);
			this.Transaction.B_EntityDataCopyForMaterial(transaction.Load());
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

			if (this.stocktakingUserIds != null)
			{
				stocktakingUsers = this.stocktakingUserIds.Select(a => new VWPRD_StocktakingUser
				{
					stocktakingId = this.id,
					userId = a,
					created = DateTime.Now,
					createdby = this.createdby
				}).ToArray();
			}

			if (this.items != null)
			{
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
					status = a.status ?? (int)EnumPRD_StocktakingItemStatus.StoklaraIslenmedi
				}), this.trans);
			}
			dbresult &= db.BulkInsertPRD_StocktakingUser(this.stocktakingUsers.Select(a => new PRD_StocktakingUser
			{
				userId = a.userId,
				stocktakingId = this.id
			}), this.trans);

			var sayimYöneticileri = db.GetVWSH_UserRoleByRoleId(Guid.Parse(SHRoles.SayimYoneticisi));
			var sayimPersonelleri = db.GetVWSH_UserRoleByRoleId(Guid.Parse(SHRoles.SayimPersoneli));

			dbresult &= db.BulkInsertSH_UserRole(this.stocktakingUsers.Where(a => sayimPersonelleri.Where(b => a.userId == b.userid).Count() < 1).Select(a => new SH_UserRole
			{
				roleid = Guid.Parse(SHRoles.SayimPersoneli),
				userid = a.userId,
				createdby = this.createdby,
				created = DateTime.Now,
			}), trans);

			if (!sayimYöneticileri.Where(a => a.userid == this.responsibleUserId).Any())
			{
				dbresult &= db.InsertSH_UserRole(new SH_UserRole
				{
					roleid = Guid.Parse(SHRoles.SayimYoneticisi),
					userid = this.responsibleUserId,
					created = DateTime.Now,
					createdby = this.createdby
				});
			}

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

			var stocktakingUsers = db.GetVWPRD_StocktakingUserByStocktakingId(this.id);
			dbresult &= db.BulkDeletePRD_StocktakingUser(stocktakingUsers.Select(a => new PRD_StocktakingUser
			{
				id = a.id,
				userId = a.userId,
				stocktakingId = this.id
			}));

			dbresult &= db.BulkInsertPRD_StocktakingUser(this.stocktakingUsers.Select(a => new PRD_StocktakingUser
			{
				userId = a.userId,
				stocktakingId = this.id
			}), this.trans);

			var sayimYöneticileri = db.GetVWSH_UserRoleByRoleId(Guid.Parse(SHRoles.SayimYoneticisi));
			var sayimPersonelleri = db.GetVWSH_UserRoleByRoleId(Guid.Parse(SHRoles.SayimPersoneli));

			dbresult &= db.BulkInsertSH_UserRole(this.stocktakingUsers.Where(a => sayimPersonelleri.Where(b => a.userId == b.userid).Count() < 1).Select(a => new SH_UserRole
			{
				roleid = Guid.Parse(SHRoles.SayimPersoneli),
				userid = a.userId,
				createdby = this.createdby,
				created = DateTime.Now,
			}), trans);

			if (!sayimYöneticileri.Where(a => a.userid == this.responsibleUserId).Any())
			{
				dbresult &= db.InsertSH_UserRole(new SH_UserRole
				{
					roleid = Guid.Parse(SHRoles.SayimYoneticisi),
					userid = this.responsibleUserId,
					created = DateTime.Now,
					createdby = this.createdby
				});
			}

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

		public static SimpleQuery UpdateQuery(SimpleQuery query, Guid userId)
		{
			BEXP filter = null;

			filter &= new BEXP
			{
				Operand1 = (COL)"responsibleUserId",
				Operator = BinaryOperator.Equal,
				Operand2 = (VAL)string.Format("{0}", userId.ToString())
			};

			filter |= new BEXP
			{
				Operand1 = (COL)"createdby",
				Operator = BinaryOperator.Equal,
				Operand2 = (VAL)string.Format("{0}", userId.ToString())
			};

			filter |= new BEXP
			{
				Operand1 = (COL)"userIds",
				Operator = BinaryOperator.Like,
				Operand2 = (VAL)string.Format("{0}", userId.ToString())
			};

			query.Filter &= filter;
			return query;
		}

		public ResultStatus InsertForStockTaking(Guid userId)
		{
			db = db ?? new WorkOfTimeDatabase();
			trans = trans ?? db.BeginTransaction();
			var outputInfo = this.GetInfo(this.Transaction.outputId, this.Transaction.outputTable, this.Transaction.outputCompanyId);
			this.Transaction.inputCompanyId = null;
			this.Transaction.inputId = null;
			this.Transaction.inputTable = null;
			this.Transaction.outputId_Title = outputInfo.Text;
			this.Transaction.outputCompanyId = outputInfo.CompanyId;
			this.Transaction.outputCompanyId_Title = outputInfo.CompanyIdTitle;
			this.Transaction.outputId = outputInfo.DataId;
			this.Transaction.outputTable = outputInfo.DataTable;
			var DBResult = new ResultStatus { result = true };
			var listOfTransItems = new List<VMPRD_TransactionItems>();



			var transModel = new VMPRD_TransactionModel
			{
				inputId = this.Transaction.inputId,
				inputTable = this.Transaction.inputTable,
				outputId = this.Transaction.outputId,
				outputTable = this.Transaction.outputTable,
				created = this.created,
				createdby = this.createdby,
				status = (int)EnumPRD_TransactionStatus.islendi,
				items = listOfTransItems,
				date = DateTime.Now,
				code = BusinessExtensions.B_GetIdCode(),
				type = (short)EnumPRD_TransactionType.HarcamaBildirimi,
				id = Guid.NewGuid()
			};

			DBResult &= transModel.Save(userId, trans);

			if (DBResult.result)
			{
				trans.Commit();
			}
			else
			{
				trans.Rollback();
			}
			return DBResult;
		}

		private OwnerInfo GetInfo(Guid? dataId, string dataTable, Guid? dataCompanyId)
		{
			OwnerInfo result = new OwnerInfo();
			db = new WorkOfTimeDatabase();
			if (dataId != null && dataTable != null)
			{
				switch (dataTable)
				{
					case "CMP_Storage":
						var storage = db.GetVWCMP_StorageById(dataId.Value);
						result = new OwnerInfo
						{
							CompanyId = storage?.companyId,
							CompanyIdTitle = storage?.companyId_Title,
							Location = storage?.location,
							Text = storage?.companyId_Title + " | " + storage?.name,
							Adress = storage?.address,
							DataId = dataId,
							DataTable = dataTable,
						};
						break;
					case "SH_User":
						var user = db.GetVWSH_UserById(dataId.Value);
						var company = db.GetVWCMP_CompanyById(user.CompanyId ?? Guid.NewGuid());
						result = new OwnerInfo
						{
							CompanyId = company?.id,
							CompanyIdTitle = company?.fullName,
							Location = company?.location,
							Text = company?.fullName + " | " + user?.FullName,
							Adress = user?.address,
							DataId = dataId,
							DataTable = dataTable,
						};
						break;
					case "CMP_Company":
						var cmp = db.GetVWCMP_CompanyById(dataId.Value);
						result = new OwnerInfo
						{
							CompanyId = cmp?.id,
							CompanyIdTitle = cmp?.fullName,
							Location = cmp.location,
							Text = cmp?.fullName,
							Adress = ""
						};
						break;
					default:
						break;
				}
			}
			else
			{
				var company = db.GetVWCMP_CompanyById(dataCompanyId ?? Guid.NewGuid());
				result.CompanyId = dataCompanyId;
				result.CompanyIdTitle = company?.fullName;
				result.Text = company?.fullName;
				result.Adress = company?.openAddress;
				result.Location = company?.location;
				result.DataId = null;
				result.DataTable = null;
			}
			return result;
		}



	}

	public class VWPRD_StockTakingItemModel : VWPRD_StocktakingItem
	{
		public int storageQuantity { get; set; }
	}
}
