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
	public class VMPRD_ProductionProductsModel : VWPRD_ProductionProduct
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		public VMPRD_ProductionProductsModel Load()
		{
			db = db ?? new WorkOfTimeDatabase();
			var productionProduct = db.GetVWPRD_ProductionProductById(this.id);

			if (productionProduct != null)
			{
				this.B_EntityDataCopyForMaterial(productionProduct, true);
			}


			return this;
		}

		public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var productionProducts = db.GetVWPRD_ProductionProductById(this.id);
			var rs = new ResultStatus { result = true };

			if (productionProducts == null)
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

			if (rs.result)
			{
				if (request != null)
				{
					new FileUploadSave(request, this.id).SaveAs();
				}
			}

			return rs;
		}

		private ResultStatus Insert(DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();
			var productionProducts = new PRD_ProductionProducts().B_EntityDataCopyForMaterial(this, true);


			var rs = db.InsertPRD_ProductionProducts(productionProducts, transaction);


			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = "Ürün Ekleme işlemi başarılı." };
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus { result = false, message = "Ürün Ekleme işlemi başarısız." };
			}
		}
		private ResultStatus Update(DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var transaction = trans ?? db.BeginTransaction();
			var productionProducts = new PRD_ProductionProducts().B_EntityDataCopyForMaterial(this, true);

			var rs = db.UpdatePRD_ProductionProducts(productionProducts, true, transaction);

			if (rs.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus { result = true, message = "Ürün Güncelleme işlemi başarılı." };
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus { result = false, message = "Ürün Güncelleme işlemi başarısız." };
			}
		}
		public ResultStatus Delete(DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var _productionProducts = db.GetPRD_ProductionProductsById(this.id);
			if (_productionProducts == null)
			{
				return new ResultStatus { result = false, message = "Ürün zaten silinmiş." };
			}

			var dbres = new ResultStatus { result = true };
			var transaction = trans ?? db.BeginTransaction();

			var productionProducts = new PRD_ProductionProducts().B_EntityDataCopyForMaterial(_productionProducts, true);
			dbres &= db.DeletePRD_ProductionProducts(productionProducts, trans);

			if (dbres.result == true)
			{
				if (trans == null) transaction.Commit();
				return new ResultStatus
				{
					result = true,
					message = "Ürün silme işlemi başarıslı oldu."
				};
			}
			else
			{
				if (trans == null) transaction.Rollback();
				return new ResultStatus
				{
					result = false,
					message = "Ürün silme işlemi başarısız oldu."
				};
			}
		}
	}
}
