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

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public VWCRM_MonthlyTargetPerson[] GetVWCRM_MonthlyTargetPersonByGroupId(Guid groupId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_MonthlyTargetPerson>().Where(a => a.GroupId == groupId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWCRM_MonthlyTargetPerson[] GetVWCRM_MonthlyTargetPersonByCPeriodPersonIds(DateTime cPeriod, Guid[] userIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_MonthlyTargetPerson>().Where(a => a.CPeriod == cPeriod && a.TargetUserId.In(userIds)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

    }
}
