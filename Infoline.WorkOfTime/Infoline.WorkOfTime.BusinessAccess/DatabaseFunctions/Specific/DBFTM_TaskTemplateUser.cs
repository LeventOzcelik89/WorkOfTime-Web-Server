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

    [EnumInfo(typeof(HDM_Issue), "status")]
    public enum EnumHDM_IssueStatus
    {
        [Description("Taslak")]
        Taslak = 0,
        [Description("Yayında")]
        Yayında = 1,
    }
    partial class WorkOfTimeDatabase
    {

        public FTM_TaskTemplateUser[] GetFTM_TaskTemplateUserByTaskTemplateId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskTemplateUser>().Where(a => a.taskTemplateId == taskId).Execute().ToArray();
            }
        }

    }
}
