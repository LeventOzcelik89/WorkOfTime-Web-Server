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

        public VWFTM_TaskFormResult[] GetVWFTM_TaskFormResultByFormId(Guid formId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskFormResult>().Where(a => a.formId == formId).Execute().ToArray();
            }
        }
        public VWFTM_TaskUser[] GetVWFTM_TaskUserByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskUser>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }

        public VWFTM_TaskUserHelper[] GetVWFTM_TaskUserHelperByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskUserHelper>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }
    }
}
