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


        public VWCRM_MonthlyTarget[] GetVWCRM_MonthlyTargetByGroupId(Guid groupId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_MonthlyTarget>().Where(a => a.GroupId == groupId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

    }
}
