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
		public VWPRD_Production Production { get; set; }
		private DbTransaction trans { get; set; }

		public VMPRD_ProductionOperationModel Load()
		{
			db = db ?? new WorkOfTimeDatabase();
			var productionOperation = db.GetVWPRD_ProductionOperationById(this.id);
			if (productionOperation != null)
			{
				this.B_EntityDataCopyForMaterial(productionOperation, true);
			}

			if (this.productionId.HasValue)
			{
				this.Production = db.GetVWPRD_ProductionById(this.productionId.Value);
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
				rs = Insert(userId);
			}
			else
			{
				this.changedby = userId;
				this.changed = DateTime.Now;
				rs = Update(userId);
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

		public ResultStatus Insert(Guid? userId)
		{
			if (this.productionId == null) return new ResultStatus { result = false, message = "Üretim kaydı seçili değil." };

			db = db ?? new WorkOfTimeDatabase();
			this.trans = this.db.BeginTransaction();
			this.Production = db.GetVWPRD_ProductionById(this.productionId.Value);
			this.created = DateTime.Now.AddSeconds(5);
			this.createdby = userId;

			if (this.Production == null) return new ResultStatus { result = false, message = "Üretim kaydı silinmiş." };
			if (this.status == null) return new ResultStatus { result = false, message = "Operasyon durumu boş gönderilemez." };

			var productionOperation = db.GetPRD_ProductionOperationByProductionId(this.productionId.Value);
			var productionUsers = db.GetPRD_ProductionUsersByProductionId(this.productionId.Value);
			var rs = new ResultStatus { result = true };
			var status = (EnumPRD_ProductionOperationStatus)this.status;
			var message = "İşlem başarıyla gerçekleşti.";

			switch (status)
			{
				case EnumPRD_ProductionOperationStatus.UretimEmriVerildi:
					break;
				case EnumPRD_ProductionOperationStatus.UretimBasladi:
					message = "Göreve başlandı.";
					break;
				case EnumPRD_ProductionOperationStatus.UretimDurduruldu:
					break;
				case EnumPRD_ProductionOperationStatus.UretimBitti:
					break;
				case EnumPRD_ProductionOperationStatus.UretimIptalEdildi:
					break;
				case EnumPRD_ProductionOperationStatus.HarcamaBildirildi:
					this.dataTable = "PRD_ProductionStage";
					message = "Envanter işlem bildirimi başarılı.";
					break;
				case EnumPRD_ProductionOperationStatus.FireBildirimiYapildi:
					break;
				case EnumPRD_ProductionOperationStatus.StogaIadeEdildi:
					break;
				case EnumPRD_ProductionOperationStatus.PersonelAtamasiYapildi:
					break;
				case EnumPRD_ProductionOperationStatus.PersonelUretimdenAlindi:
					break;
				case EnumPRD_ProductionOperationStatus.FormYuklendi:
					break;
				default:
					break;
			}

			rs &= db.InsertPRD_ProductionOperation(new PRD_ProductionOperation().B_EntityDataCopyForMaterial(this, true));


			if (rs.result == true)
			{
				if (trans == null) trans.Commit();
				return new ResultStatus { result = true, message = message };
			}
			else
			{
				if (trans == null) trans.Rollback();
				return new ResultStatus { result = false, message = "İşlem başarısız." };
			}
		}
		public ResultStatus Update(Guid? userId)
		{
			this.db = new WorkOfTimeDatabase();
			this.trans = this.db.BeginTransaction();
			this.changedby = userId;
			this.changed = DateTime.Now;

			var rs = db.UpdatePRD_ProductionOperation(new PRD_ProductionOperation().B_EntityDataCopyForMaterial(this, true), false, trans);

			if (rs.result)
			{
				rs.message = "Güncelleme işlemi başarılı.";
				trans.Commit();
			}
			else
			{
				rs.message = "Güncelleme işlemi başarısız.";
				trans.Rollback();
			}
			return rs;
		}
	}
}
