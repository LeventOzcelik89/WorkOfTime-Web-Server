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
	public class VMPRD_ProductBountyModel : VWPRD_ProductBounty
	{

		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		public Guid[] personIds { get; set; }

		public VMPRD_ProductBountyModel Load()
		{

			db = db ?? new WorkOfTimeDatabase();
			var productBounty = db.GetVWPRD_ProductBountyById(this.id);

			if (productBounty != null)
			{
				this.B_EntityDataCopyForMaterial(productBounty, true);
			}

			return this;
		}
		public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
		{

			db = db ?? new WorkOfTimeDatabase();
			var productBounty = db.GetVWPRD_ProductBountyById(this.id);
			var rs = new ResultStatus { result = true };

			if (productBounty == null)
			{
				this.createdby = userId;
				this.created = DateTime.Now;
				rs = Insert(trans);
			}
			else
			{
				this.changedby = userId;
				this.changed = DateTime.Now;
				rs = Update(trans);
			}

			return rs;
		}

		private ResultStatus Insert(DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();

			if (this.productId.HasValue)
			{
				var productBounty = db.GetPRD_ProductBountyByPeriodAndPersonId(this.month.Value, this.year.Value, this.productId.Value, this.personIds);

				if (productBounty.Count() > 0)
				{
					return new ResultStatus { result = false, message = "Belirtilmiş olan dönem içerisin de personel(lere) daha önceden prim tanımlaması yapılmıştır." };
				}
			}


			var productBountys = this.personIds.Select(a => new PRD_ProductBounty
			{
				amount = this.amount,
				personId = a,
				productId = this.productId,
				companyId = this.companyId,
				month = this.month,
				year = this.year
			});

			var rs = db.BulkInsertPRD_ProductBounty(productBountys, transaction);

			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = "Ürün Prim Tanımlama İşlemi Başarılı." };
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus { result = false, message = "Ürün Prim Tanımlama İşlemi başarısız." };
			}
		}
		private ResultStatus Update(DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();
			var productBounty = new PRD_ProductBounty().B_EntityDataCopyForMaterial(this, true);

			var rs = db.UpdatePRD_ProductBounty(productBounty, true, transaction);

			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = "Ürün Prim Güncelleme işlemi başarılı." };
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus { result = false, message = "Ürün Prim Güncelleme işlemi başarısız." };
			}
		}
		public ResultStatus Delete(DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();
			var _productBounty = db.GetPRD_ProductBountyById(this.id);
			if (_productBounty == null)
			{
				return new ResultStatus { result = false, message = "Ürün prim tanımı silinmiş." };
			}

			var dbres = new ResultStatus { result = true };
			dbres &= db.DeletePRD_ProductBounty(_productBounty);

			if (dbres.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus
				{
					result = true,
					message = "Ürün Prim silme işlemi başarılı oldu."
				};
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus
				{
					result = false,
					message = "Ürün Prim silme işlemi başarısız oldu."
				};
			}
		}
	}
}
