using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_BountyModel : VWPRD_Bounty
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public double? count { get; set; } = 0;
        public double? price { get; set; } = 0;

        public VMPRD_BountyModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var bounty = db.GetVWPRD_BountyById(this.id);
            if (bounty != null)
            {
                this.B_EntityDataCopyForMaterial(bounty, true);
            }
            return this;
        }
        public ResultStatus Approve(VMPRD_BountyModel model, Guid? userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var data = db.GetPRD_BountyById(model.id);
            if (data == null)
            {
                result.result = false;
                result.message = "Hakediş Bulunamadı.";
                return result;
            }
            if (data.status == (int)EnumPRD_BountyStatus.completePayment)
            {
                result.result = false;
                result.message = "Hakediş Daha Önce Onaylanmış.";
                return result;
            }
            data.status = (int)EnumPRD_BountyStatus.completePayment;
            data.changed = DateTime.Now;
            data.changedby = userId;
            result = db.UpdatePRD_Bounty(data);
            result.message = "İşlem Onaylandı";
            return result;
        }

        public List<PRD_Bounty> ReportData(Guid companyId, Guid bonusId, int status)
        {
            db = db ?? new WorkOfTimeDatabase();
            var list = new List<PRD_Bounty>();
            var data = db.GetPRD_Bounty();
            if (companyId != null && bonusId != null && status != null)
            {
                data = db.GetPRD_Bounty().Where(a => a.companyId == companyId && a.productBonusId == bonusId && a.status == (int)status).ToArray();
            }
            else if (companyId != null && bonusId != null && status == null)
            {
                data = db.GetPRD_Bounty().Where(a => a.companyId == companyId && a.productBonusId == bonusId).ToArray();
            }
            else if (companyId != null && bonusId == null && status == null)
            {
                data = db.GetPRD_Bounty().Where(a => a.companyId == companyId).ToArray();
            }
            else if (companyId == null && bonusId != null && status != null)
            {
                data = db.GetPRD_Bounty().Where(a => a.productBonusId == bonusId && a.status == (int)status).ToArray();
            }
            else if (companyId == null && bonusId == null && status != null)
            {
                data = db.GetPRD_Bounty().Where(a => a.status == (int)status).ToArray();
            }
            else
            {
                data = db.GetPRD_Bounty();
            }
            list.AddRange(data);
            return list;
        }
    }

}
