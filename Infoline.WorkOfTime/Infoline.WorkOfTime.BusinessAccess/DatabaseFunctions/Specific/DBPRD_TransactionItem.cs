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

        public PRD_TransactionItem[] GetPRD_TransactionItemByTransactionId(Guid transactionId)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_TransactionItem>().Where(a => a.transactionId == transactionId).Execute().ToArray();
            }
        }

        public PRD_TransactionItem[] GetPRD_TransactionItemByTransactionIds(Guid[] transactionIds)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_TransactionItem>().Where(a => a.transactionId.In(transactionIds)).Execute().ToArray();
            }
        }
        public PRD_TransactionItem[] GetPRD_TransactionItemByProductIds(Guid[] productIds )
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_TransactionItem>().Where(a => a.productId.In(productIds)).Execute().ToArray();
            }
        }

    }
}
