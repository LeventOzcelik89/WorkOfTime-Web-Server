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
        public SH_Group GetSH_GroupByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_Group>().Where(s => s.code == code).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}
