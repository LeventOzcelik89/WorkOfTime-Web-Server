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
        public VWPRD_TransactionItem[] GetVWPRD_TransactionItemByTransactionId(Guid transactionId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRD_TransactionItem>().Where(a => a.transactionId == transactionId).Execute().ToArray();
            }
        }

        public VWPRD_TransactionItem[] GetVWPRD_TransactionItemByTransactionIds(Guid[] transactionIds)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRD_TransactionItem>().Where(a => a.transactionId.In(transactionIds)).Execute().ToArray();
            }
        }

    }
}
