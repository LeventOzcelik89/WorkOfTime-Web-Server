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

        public VWSH_GroupUsers[] GetVWSH_GroupUsersByUserIds(Guid[] userIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_GroupUsers>().Where(a => a.userId.In(userIds)).OrderBy(a => a.created).Execute().ToArray();
            }
        }

        public VWSH_GroupUsers GetVWSH_GroupUserByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_GroupUsers>().Where(a => a.userId == userId).OrderBy(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}
