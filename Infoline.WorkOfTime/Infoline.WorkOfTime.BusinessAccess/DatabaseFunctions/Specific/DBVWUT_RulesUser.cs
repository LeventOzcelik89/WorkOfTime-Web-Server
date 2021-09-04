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

        public VWUT_RulesUser GetVWUT_RulesUserByUserIdAndType(Guid userId, int type)
        {
            using (var db = GetDB())
            {
                return db.Table<VWUT_RulesUser>().Where(a => a.userId == userId && a.ruleType == type).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
        public VWUT_RulesUser[] GetVWUT_RulesUserByUserIdAndRulesIdNot(Guid id, Guid ruleId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWUT_RulesUser>().Where(x => x.userId == id && (x.rulesId == ruleId || x.rulesId != ruleId)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
    }
}