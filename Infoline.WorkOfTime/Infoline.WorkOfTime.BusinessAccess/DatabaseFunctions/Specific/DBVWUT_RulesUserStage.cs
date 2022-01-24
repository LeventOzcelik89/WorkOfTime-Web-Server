using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {

        public VWUT_RulesUserStage[] GetVWUT_RulesUserStageByRulesId(Guid ruleId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWUT_RulesUserStage>().Where(a => a.rulesId == ruleId).OrderBy(a => a.order).Execute().ToArray();
            }
        }


        public VWUT_RulesUserStage GetVWUT_RulesUserStageByOrderAndTypeAndRulesId(short order, Guid rulesId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWUT_RulesUserStage>().Where(a => a.order == order && a.rulesId == rulesId).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}