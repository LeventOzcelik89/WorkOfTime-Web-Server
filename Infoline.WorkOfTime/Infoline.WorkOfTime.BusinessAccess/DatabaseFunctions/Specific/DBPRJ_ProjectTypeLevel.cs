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
        public VWPRJ_ProjectTypeLevel[] GetVWPRJ_ProjectTypeLevelByProjectId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectTypeLevel>().Where(a => a.projectId == projectId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWPRJ_ProjectTypeLevel GetVWPRJ_ProjectTypeLevelByProjectIdAndType(Guid projectId,int type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectTypeLevel>().Where(a => a.projectId == projectId && a.type == type).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public VWPRJ_ProjectTypeLevel GetVWPRJ_ProjectTypeLevelByProjectIdAndTypeAndId(Guid projectId, int type,Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectTypeLevel>().Where(a => a.projectId == projectId && a.type == type && a.id != id).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public VWPRJ_ProjectScopeLevel[] GetVWPRJ_ProjectScopeLevelScopeLevelByContractId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectScopeLevel>().Where(a => a.projectId == projectId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }


        public PRJ_ProjectServiceLevel[] GetPRJ_ProjectServiceLevelByContractId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRJ_ProjectServiceLevel>().Where(a => a.projectId == projectId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
    }
}
