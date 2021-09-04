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
	public class VMPRD_ProductionOperationModel : VWPRD_ProductionOperation
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		public VMPRD_ProductionOperationModel Load()
		{
			db = db ?? new WorkOfTimeDatabase();
			var productionOperation = db.GetVWPRD_ProductionOperationById(this.id);
			if (productionOperation != null)
			{
				this.B_EntityDataCopyForMaterial(productionOperation, true);
			}
			return this;
		}

		public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var productionOperation = db.GetVWPRD_ProductionOperationById(this.id);
			var rs = new ResultStatus { result = true };

			if (productionOperation == null)
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
			var productionOperation = new PRD_ProductionOperation().B_EntityDataCopyForMaterial(this, true);

			var rs = db.InsertPRD_ProductionOperation(productionOperation, transaction);


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
			var productionOperation = new PRD_ProductionOperation().B_EntityDataCopyForMaterial(this, true);
			var rs = db.UpdatePRD_ProductionOperation(productionOperation, true, transaction);

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
			var _productionOperation = db.GetPRD_ProductionOperationById(this.id);
			if (_productionOperation == null)
			{
				return new ResultStatus { result = false, message = "Ürün zaten silinmiş." };
			}

			var dbres = new ResultStatus { result = true };
			var transaction = trans ?? db.BeginTransaction();

			var productionOperation = new PRD_ProductionOperation().B_EntityDataCopyForMaterial(_productionOperation, true);
			dbres &= db.DeletePRD_ProductionOperation(productionOperation, trans);

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
