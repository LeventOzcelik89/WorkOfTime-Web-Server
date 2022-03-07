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
        public VWPRD_ProductBonus GetVWPRD_ProductBonusByRuleName(string ruleName, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductBonus>().Where(a => a.ruleName == ruleName).Execute().FirstOrDefault();
            }
        }
    }
}
