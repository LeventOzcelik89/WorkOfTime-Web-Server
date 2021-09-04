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

        public VWCMP_InvoiceItem[] GetVWCMP_InvoiceItemByInvoiceId(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_InvoiceItem>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
            }
        }

        public VWCMP_InvoiceItem[] GetVWCMP_InvoiceItemByInvoiceIds(Guid[] invoiceIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_InvoiceItem>().Where(a => a.invoiceId.In(invoiceIds)).Execute().ToArray();
            }
        }

    }
}
