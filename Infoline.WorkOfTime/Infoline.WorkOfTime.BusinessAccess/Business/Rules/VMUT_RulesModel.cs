using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMUT_RulesModel : VWUT_Rules
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		public VWUT_RulesUserStage[] rulesUserStage { get; set; }
		public VWUT_RulesUser rulesUser { get; set; }

		public VMUT_RulesModel Load()
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var rules = db.GetVWUT_RulesById(this.id);

			if (rules != null)
			{
				this.B_EntityDataCopyForMaterial(rules, true);
			}

			return this;
		}


		public ResultStatus Save(Guid userId, DbTransaction trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var rules = db.GetVWUT_RulesById(this.id);
			var res = new ResultStatus { result = true };


			if (rules == null)
			{
				this.createdby = userId;
				this.created = DateTime.Now;
				res = Insert(trans);
			}
			else
			{
				this.changedby = userId;
				this.changed = DateTime.Now;
				res = Update(trans);

			}

			if (trans == null && res.result)
			{
				this.trans.Commit();
			}
			else
			{
				this.trans.Rollback();
			}

			return res;
		}

		private ResultStatus Insert(DbTransaction trans = null)
		{
			this.trans = trans ?? db.BeginTransaction();

			var dbresult = db.InsertUT_Rules(new UT_Rules().B_EntityDataCopyForMaterial(this), this.trans);

			if (!dbresult.result)
			{

				return new ResultStatus
				{
					result = false,
					message = "Kural kaydetme işlemi başarısız."
				};
			}
			else
			{
				return new ResultStatus
				{
					result = true,
					message = "Kural kaydetme işlemi başarılı."
				};
			}

		}

		private ResultStatus Update(DbTransaction trans = null)
		{
			this.trans = trans ?? db.BeginTransaction();
			var rules = db.GetUT_RulesById(this.id);

			rules.changed = DateTime.Now;
			rules.changedby = this.changedby;
			rules.isDefault = this.isDefault;
			rules.name = this.name;
			rules.type = this.type;


			var dbresult = db.UpdateUT_Rules(rules, false, this.trans);

			if (!dbresult.result)
			{

				return new ResultStatus
				{
					result = false,
					message = "Kural güncelleme işlemi başarısız."
				};
			}
			else
			{
				return new ResultStatus
				{
					result = true,
					message = "Kural güncelleme işlemi başarılı."
				};
			}
		}


	}

}
