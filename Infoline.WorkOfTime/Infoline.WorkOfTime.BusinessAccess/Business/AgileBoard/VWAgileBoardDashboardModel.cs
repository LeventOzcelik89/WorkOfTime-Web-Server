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

    public class VWAgileBoardDashboardModel
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public Guid? id { get; set; }

        public VWSH_AgileBoards AgileBoard { get; set; } = new VWSH_AgileBoards();
        public List<VWSH_AgileBoards> MyAgileBoards { get; set; } = new List<VWSH_AgileBoards>();
        public List<VWCRM_ManagerStage> Stages { get; set; } = new List<VWCRM_ManagerStage>();
        public List<Guid> SalesPersons { get; set; } = new List<Guid> { };

        public VWAgileBoardDashboardModel Load(Guid userId)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();

            this.Stages = db.GetVWCRM_ManagerStage().ToList();
            this.MyAgileBoards = db.GetVWSH_AgileBoardsByUserId(userId).OrderByDescending(a => a.lastUsedDate).ToList();

            var satisPersonelleri = db.GetUserByRoleId(SHRoles.SatisPersoneli).ToList();
            this.SalesPersons = db.GetVWSH_UserMyPersonIsWorkingIDS(satisPersonelleri).Select(a => a.id).ToList();
            this.SalesPersons.Add(userId);

            if (this.id.HasValue)
            {
                this.AgileBoard = this.MyAgileBoards.Where(a => a.id == this.id).FirstOrDefault();
            }
            else
            {
                this.AgileBoard = this.MyAgileBoards.OrderByDescending(a => a.lastUsedDate).FirstOrDefault();
            }

            return this;
        }

        //public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        //{

        //    db = db ?? new WorkOfTimeDatabase();
        //    var product = db.GetVWSH_AgileBoardsById(this.id);
        //    var rs = new ResultStatus { result = true };

        //    if (product == null)
        //    {
        //        this.createdby = userId;
        //        this.created = DateTime.Now;
        //        rs = Insert(trans);
        //    }
        //    else
        //    {
        //        this.changedby = userId;
        //        this.changed = DateTime.Now;
        //        rs = Update(trans);
        //    }

        //    if (rs.result)
        //    {
        //        if (request != null)
        //        {
        //            new FileUploadSave(request, this.id).SaveAs();
        //        }
        //    }

        //    return rs;
        //}

        //private ResultStatus Insert(DbTransaction trans = null)
        //{
        //    db = db ?? new WorkOfTimeDatabase();
        //    var transaction = trans ?? db.BeginTransaction();
        //    var agile = this.B_ConvertType<SH_AgileBoards>();

        //    var rs = db.InsertSH_AgileBoards(agile, transaction);

        //    if (rs.result == true)
        //    {
        //        if (trans == null) transaction.Commit();
        //        return new ResultStatus { result = true, message = "Agile Board başarıyla kaydedildi." };
        //    }
        //    else
        //    {
        //        if (trans == null) transaction.Rollback();
        //        return new ResultStatus { result = false, message = rs.message };
        //    }
        //}

        //private ResultStatus Update(DbTransaction trans = null)
        //{
        //    db = db ?? new WorkOfTimeDatabase();
        //    var transaction = trans ?? db.BeginTransaction();
        //    var agile = new PRD_Product().B_EntityDataCopyForMaterial(this, true);

        //    var rs = db.UpdatePRD_Product(agile, true, transaction);

        //    if (rs.result == true)
        //    {
        //        if (trans == null) transaction.Commit();
        //        return new ResultStatus { result = true, message = "Agile Board güncelleme işlemi başarıyla tamamlandı." };
        //    }
        //    else
        //    {
        //        if (trans == null) transaction.Rollback();
        //        return new ResultStatus { result = false, message = rs.message };
        //    }
        //}

        //public ResultStatus Delete(DbTransaction trans = null)
        //{
        //    db = db ?? new WorkOfTimeDatabase();
        //    var agile = db.GetSH_AgileBoardsById(this.id);
        //    if (agile == null)
        //    {
        //        return new ResultStatus { result = false, message = "Agile Board zaten silinmiş." };
        //    }

        //    var dbres = new ResultStatus { result = true };
        //    var transaction = trans ?? db.BeginTransaction();

        //    dbres &= db.DeleteSH_AgileBoards(agile, trans);

        //    if (dbres.result == true)
        //    {
        //        if (trans == null) transaction.Commit();
        //        return new ResultStatus
        //        {
        //            result = true,
        //            message = "Agile Board silme işlemi başarıyla tamamlandı."
        //        };
        //    }
        //    else
        //    {
        //        if (trans == null) transaction.Rollback();
        //        return new ResultStatus
        //        {
        //            result = false,
        //            message = dbres.message
        //        };
        //    }
        //}

    }
}
