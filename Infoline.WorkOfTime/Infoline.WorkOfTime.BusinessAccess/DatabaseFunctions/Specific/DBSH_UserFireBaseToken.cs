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
        public SH_UserFireBaseToken GetSH_UserFireBaseTokenByUserId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserFireBaseToken>().Where(s => s.userId == id).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public ResultStatus UpdateSH_UserFireBaseTokenByUserId(SH_UserFireBaseToken item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<SH_UserFireBaseToken>(item);
            }
        }
    }
}
