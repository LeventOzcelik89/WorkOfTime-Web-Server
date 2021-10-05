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
        public PA_TransactionConfirmation[] GetPA_TransactionConfirmationByTransactionId(Guid transactionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_TransactionConfirmation>().Where(a => a.transactionId == transactionId).Execute().ToArray();
            }
        }
    }
}
