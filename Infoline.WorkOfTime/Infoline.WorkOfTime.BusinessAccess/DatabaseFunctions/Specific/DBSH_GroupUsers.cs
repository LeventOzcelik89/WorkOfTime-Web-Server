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
        public SH_GroupUsers[] GetSH_GroupUsersByGroupId(Guid groupId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_GroupUsers>().Where(a => a.groupId == groupId).OrderBy(a => a.created).Execute().ToArray();
            }
        }
    }
}
