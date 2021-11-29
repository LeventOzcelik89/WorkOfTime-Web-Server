using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_EntegrationImport : VWPRD_EntegrationImport
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMPRD_EntegrationImport Load()
        {
            this.db = db ?? new WorkOfTimeDatabase();
            var data = db.GetVWPRD_EntegrationImportById(this.id);
            if (data != null)
            {
                return this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, DbTransaction transaction = null)
        {
            var result = new ResultStatus();
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWPRD_EntegrationImportById(this.id);
            if (data != null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                result = Insert();
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                result = Update();
            }
            if (result.result)
            {
                if (transaction == null)
                    this.trans.Commit();
                return result;
            }
            else
            {
                if (transaction == null)
                    this.trans.Rollback();
            }
            return result;
        }
        private ResultStatus Update()
        {
            var result = new ResultStatus();
            db = db ?? new WorkOfTimeDatabase();
            result &= db.UpdatePRD_EntegrationImport(new PRD_EntegrationImport().B_EntityDataCopyForMaterial(this));
            if (!result.result)
            {
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Dosya güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Dosyadan güncelleme işlemi başarılı oldu."
                };
            }
        }
        private ResultStatus Insert()
        {
            var result = new ResultStatus();
            db = db ?? new WorkOfTimeDatabase();
            result &= db.InsertPRD_EntegrationImport(new PRD_EntegrationImport().B_EntityDataCopyForMaterial(this));
            if (!result.result)
            {
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Dosya ekleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Dosyadan ekleme işlemi başarılı oldu."
                };
            }
        }
    }
}
