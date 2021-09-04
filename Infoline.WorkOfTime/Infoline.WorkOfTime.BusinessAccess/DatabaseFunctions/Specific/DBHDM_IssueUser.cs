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
        public HDM_IssueUser[] GetHDM_IssueUserByIssueId(Guid issueId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_IssueUser>().Where(a => a.issueId == issueId).Execute().ToArray();
            }
        }

        public HDM_IssueUser[] GetHDM_IssueUserByIssueIds(Guid[] issueIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_IssueUser>().Where(a => a.issueId.In(issueIds)).Execute().ToArray();
            }
        }

        public HDM_IssueUser[] GetHDM_IssueUserByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_IssueUser>().Where(a => a.userId == userId).Execute().ToArray();
            }
        }
    }
}
