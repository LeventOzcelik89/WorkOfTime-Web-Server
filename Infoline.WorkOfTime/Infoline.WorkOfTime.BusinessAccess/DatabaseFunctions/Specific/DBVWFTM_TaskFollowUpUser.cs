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
        public VWFTM_TaskFollowUpUser[] GetVWFTM_TaskFollowUpUserByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskFollowUpUser>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }

        public VWFTM_TaskFollowUpUser[] GetVWFTM_TaskFollowUpUserByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskFollowUpUser>().Where(a => a.userId == userId).Execute().ToArray();
            }
        }
    }
}
