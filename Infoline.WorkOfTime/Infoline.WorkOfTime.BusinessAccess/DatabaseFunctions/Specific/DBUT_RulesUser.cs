using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

    partial class WorkOfTimeDatabase
    {

        public UT_RulesUser GetUT_RulesUserByUserIdAndRulesId(Guid id, Guid ruleId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_RulesUser>().Where(x => x.userId == id && x.rulesId == ruleId).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }


        public UT_RulesUser[] GetUT_RulesUserByRuleId(Guid ruleId)
        {
            using (var db = GetDB())
            {
                return db.Table<UT_RulesUser>().Where(a => a.rulesId == ruleId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }


        public UT_RulesUser[] GetUT_RulesUserByUserId(Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<UT_RulesUser>().Where(a => a.userId == userId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

    }
}
