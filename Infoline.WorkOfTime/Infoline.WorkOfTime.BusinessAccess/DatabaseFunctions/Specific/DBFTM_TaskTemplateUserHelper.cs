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
        public FTM_TaskTemplateUserHelper[] GetFTM_TaskTemplateUserHelperByTaskTemplateId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskTemplateUserHelper>().Where(a => a.taskTemplateId == taskId).Execute().ToArray();
            }
        }

    }
}
