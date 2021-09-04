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
        public VWPA_Ledger[] GetVWPA_LedgerbyAccountId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPA_Ledger>().Where(a => a.accountId == id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWPA_Ledger[] GetVWPA_LedgerbyAccountIds(Guid[] ids,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPA_Ledger>().Where(a => a.accountId.In(ids)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
    }
}
