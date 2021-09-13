using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class VWAgileBoardModel : VWSH_AgileBoards
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public bool IsUpdate { get; set; }

        public VWAgileBoardModel Load(Guid? userId = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var agile = db.GetVWSH_AgileBoardsById(this.id);
            if (agile != null)
            {
                this.B_EntityDataCopyForMaterial(agile, true);
                this.IsUpdate = true;
            }
            else
            {
                this.IsUpdate = false;
            }

            return this;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {

            db = db ?? new WorkOfTimeDatabase();
            var product = db.GetVWSH_AgileBoardsById(this.id);
            var rs = new ResultStatus { result = true };

            this.userId = userId;
            this.lastUsedDate = DateTime.Now;

            if (product == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                this.lastUsedDate = DateTime.Now;

                rs = Insert(trans);
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                this.lastUsedDate = DateTime.Now;

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
            var agile = this.B_ConvertType<SH_AgileBoards>();

            var rs = db.InsertSH_AgileBoards(agile, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Agile Board başarıyla kaydedildi." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = rs.message };
            }
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var agile = new SH_AgileBoards().B_EntityDataCopyForMaterial(this, true);

            var rs = db.UpdateSH_AgileBoards(agile, true, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Agile Board güncelleme işlemi başarıyla tamamlandı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = rs.message };
            }
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var agile = db.GetSH_AgileBoardsById(this.id);
            if (agile == null)
            {
                return new ResultStatus { result = false, message = "Agile Board zaten silinmiş." };
            }

            var dbres = new ResultStatus { result = true };
            var transaction = trans ?? db.BeginTransaction();

            dbres &= db.DeleteSH_AgileBoards(agile, trans);

            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Agile Board silme işlemi başarıyla tamamlandı."
                };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = dbres.message
                };
            }
        }

    }
}
