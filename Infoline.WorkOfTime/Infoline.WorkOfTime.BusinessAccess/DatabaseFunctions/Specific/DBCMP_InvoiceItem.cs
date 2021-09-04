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



    [EnumInfo(typeof(CMP_InvoiceItem), "discountType")]
    public enum EnumCMP_InvoiceItemDiscountType
    {
        [Description("%")]
        Yuzde = 0,
        [Description("₺")]
        Tutar = 1,
    }


    [EnumInfo(typeof(CMP_InvoiceItem), "KDV")]
    public enum EnumCMP_InvoiceItemKDV
    {
        [Description("18%")]
        Yuzde18 = 0,
        [Description("8%")]
        Yuzde8 = 1,
        [Description("1%")]
        Yuzde1 = 2,
        [Description("0%")]
        Yuzde0 = 3,
    }


    partial class WorkOfTimeDatabase
    {
      
        public CMP_InvoiceItem[] GetCMP_InvoiceItemByInvoiceId(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_InvoiceItem>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
            }
        }

        public int GetCMP_InvoiceItemByProductIdCount(Guid productId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_InvoiceItem>().Where(a => a.productId == productId).Count();
            }
        }

    }
}
