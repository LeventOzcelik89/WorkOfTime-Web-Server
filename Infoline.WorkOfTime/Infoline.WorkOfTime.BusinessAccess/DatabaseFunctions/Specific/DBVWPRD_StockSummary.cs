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
        public VWPRD_StockSummary[] GetVWPRD_StockSummaryByStockId(Guid stockId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_StockSummary>().Where(a => a.stockId == stockId).Execute().ToArray();
            }
        }

        public VWPRD_StockSummary[] GetVWPRD_StockSummaryHasDebitByStockId(Guid stockId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_StockSummary>().Where(a => a.stockId == stockId && a.stockTable=="SH_User" && a.quantity!=0).Execute().ToArray();
            }
        }


        public VWPRD_StockSummary[] GetVWPRD_StockSummaryByStockIds(Guid[] stockIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_StockSummary>().Where(a => a.stockId.In(stockIds)).Execute().ToArray();
            }
        }

        public VWPRD_StockAction[] GetVWPRD_StockActionByStockIds(Guid[] stockIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_StockAction>().Where(a => a.stockId.In(stockIds)).Execute().ToArray();
            }
        }

        public VWPRD_StockSummary[] GetVWPRD_StockSummaryByProductIdsAndStockId(Guid[] productIds, Guid stockId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_StockSummary>().Where(a => a.productId.In(productIds) && a.stockId == stockId && a.stockTable == "CMP_Storage" && a.quantity > 0).Execute().ToArray();
            }
        }
    }
}
