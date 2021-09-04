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
        public CMP_InvoiceTransform[] GetCMP_InvoiceTransformByIsTransformedFrom(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_InvoiceTransform>().Where(a => a.invoiceIdFrom == invoiceId).Execute().ToArray();
            }
        }

        public CMP_InvoiceTransform[] GetCMP_InvoiceTransformByIsTransformedTo(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_InvoiceTransform>().Where(a => a.invoiceIdTo == invoiceId).Execute().ToArray();
            }
        }

    }
}
