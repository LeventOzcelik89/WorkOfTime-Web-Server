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
        public PRJ_ProjectScopeLevelItems[] GetPRJ_ProjectScopeLevelItemsByScopeLevelId(Guid scopeLevelId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRJ_ProjectScopeLevelItems>().Where(a => a.scopeLevelId == scopeLevelId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWPRJ_ProjectScopeLevelItems[] GetVWPRJ_ProjectScopeLevelItemsByProjectId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectScopeLevelItems>().Where(a => a.projectId == projectId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
    }
}
