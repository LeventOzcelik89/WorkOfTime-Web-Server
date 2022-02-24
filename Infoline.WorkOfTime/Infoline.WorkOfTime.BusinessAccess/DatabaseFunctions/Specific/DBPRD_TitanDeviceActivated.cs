using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Text;
namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public PRD_TitanDeviceActivated GetPRD_TitanDeviceActivatedBySerialCode(string id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.SerialNumber == id).Execute().FirstOrDefault();
            }
        }
        public PRD_TitanDeviceActivated[] GetPRD_TitanDeviceActivatedByProductId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.ProductId == id).Execute().ToArray();
            }
        }
        public PRD_TitanDeviceActivated[] GetPRD_TitanDeviceActivatedBySerialNoOrImei(string[] serialNo, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.SerialNumber.In(serialNo)||a.IMEI1.In(serialNo)||a.IMEI2.In(serialNo)).Execute().ToArray();
            }
        }
        public PRD_TitanDeviceActivated GetPRD_TitanDeviceActivatedByInventoryId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.InventoryId == id).Execute().FirstOrDefault();
            }
        }
        public int GetPRD_TitanDeviceActivatedCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x => x.InventoryId != null && x.ProductId != null).Execute().Count();
            }
        }
        public int GetPRD_TitanDeviceActivatedTodayCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                return db.Table<PRD_TitanDeviceActivated>().Where(x => x.CreatedOfTitan >= today&& x.InventoryId != null && x.ProductId != null).Execute().Count();
            }
        }
        public int GetPRD_TitanDeviceActivatedSevenDaysCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x => x.CreatedOfTitan > DateTime.Now.AddDays(-7)&& x.InventoryId != null&&x.ProductId!=null).Execute().Count();
            }
        }
        public int GetPRD_TitanDeviceActivatedThirtyDaysCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x => x.CreatedOfTitan > DateTime.Now.AddDays(-30)&& x.InventoryId != null && x.ProductId != null).Execute().Count();
            }
        }
        public DateTime? GetPRD_TitanDeviceActivatedGetAllLastDate(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().OrderByDesc(x => x.CreatedOfTitan).Execute().Select(x => x.CreatedOfTitan).FirstOrDefault();
            }
        }
        public SellOutReportModel[] GetPRD_TitanDeviceActivatedSellOutProduct(DateTime startDate,DateTime endDate)
        {
            using (var db = GetDB()){
                return db.Table<VWPRD_TitanDeviceActivated>().Where(x => x.productId_Title!=null&&x.CreatedOfTitan!=null && x.InventoryId != null && x.CreatedOfTitan >= startDate.Date && x.CreatedOfTitan <= endDate.Date).Execute().ToList().
                   GroupBy(a => a.productId_Title).Select(b => new SellOutReportModel { Count = b.Count(), Name = b.Key })
                   .ToArray();
            }
        }

        public SellOutReportModel[] GetPRD_TitanDeviceActivatedSellOutProductQuery(DateTime startDate, DateTime endDate)
        {
            using (var db = GetDB())
            {
                StringBuilder cc = new StringBuilder();
                cc.AppendLine("SELECT [dbo].[fn_PRD_ProductFullName](ProductId) as Name, count(ProductId) as Count");
                cc.AppendLine("FROM PRD_TitanDeviceActivated with(nolock)");
                cc.AppendLine(string.Format("WHERE ProductId is not null AND CreatedOfTitan >= '{0}' AND CreatedOfTitan <= '{1}' AND InventoryId is not null", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));
                cc.AppendLine("GROUP BY ProductId");
                cc.AppendLine("ORDER BY count(ProductId) DESC");

                return db.ExecuteReader<SellOutReportModel>(cc.ToString()).ToArray();
            }
        }
        public SellOutReportModel[] GetPRD_TitanDeviceActivatedSellOutDist(Guid[]ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.id.In(ids)&&x.lastActionDataCompanyId_Title!=null).Execute().ToList().GroupBy(x=>x.lastActionDataCompanyId_Title).Select(x=>new SellOutReportModel
                {
                    Count = x.Count(),
                    Name = x.Key,
                    Id = x.FirstOrDefault().lastActionDataCompanyId,
                    Types = x.FirstOrDefault().lastActionCompanyTitles,
                    SellingCount = 0
                }).ToArray();
            }
        }

        public SellOutReportModel[] GetPRD_TitanDeviceActivatedSellOutDistQuery(DateTime startDate, DateTime endDate)
        {
            using (var db = GetDB())
            {
                StringBuilder cc = new StringBuilder();
                cc.AppendLine("SELECT lastActionDataCompanyId as Id, lastActionDataCompanyId_Title as Name, lastActionCompanyTitles as Types, COUNT(lastActionDataCompanyId_Title) as Count, (select 0) as SellingCount");
                cc.AppendLine("FROM VWPRD_Inventory with(nolock)");
                cc.AppendLine("WHERE id in (SELECT InventoryId");
                cc.AppendLine("FROM PRD_TitanDeviceActivated");
                cc.AppendLine(string.Format("WHERE ProductId is not null AND CreatedOfTitan >= '{0}' AND CreatedOfTitan <= '{1}' AND InventoryId is not null)", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));
                cc.AppendLine("AND lastActionDataCompanyId_Title is not null");
                cc.AppendLine("GROUP BY lastActionDataCompanyId, lastActionDataCompanyId_Title, lastActionCompanyTitles");
                cc.AppendLine("ORDER BY COUNT(lastActionDataCompanyId) DESC");

                return db.ExecuteReader<SellOutReportModel>(cc.ToString()).ToArray();
            }
        }

        public VWPRD_Inventory[] GetPRD_TitanDeviceActivatedSellOutChartInventoryData(Guid[] ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.id.In(ids) && x.lastActionDataCompanyId_Title != null).Execute().ToArray();
            }
        }

        public VWPRD_Inventory[] GetPRD_TitanDeviceActivatedSellOutChartInventoryDataQuery(DateTime startDate, DateTime endDate)
        {
            using (var db = GetDB())
            {
                StringBuilder cc = new StringBuilder();
                cc.AppendLine("SELECT t.id, t.created, [dbo].[fn_CMP_CompanyCodeNameById](l.dataCompanyId) as lastActionDataCompanyId_Title");
                cc.AppendLine("FROM PRD_Inventory as t WITH ( NOLOCK ) OUTER APPLY (Select top 1 * from dbo.PRD_InventoryAction  AS a WITH ( NOLOCK ) where inventoryId = t.id order by created desc) as l");
                cc.AppendLine("WHERE t.id in (SELECT InventoryId");
                cc.AppendLine("FROM PRD_TitanDeviceActivated");
                cc.AppendLine(string.Format("WHERE ProductId is not null AND CreatedOfTitan >= '{0}' AND CreatedOfTitan <= '{1}' AND InventoryId is not null)", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));
                cc.AppendLine("AND l.dataCompanyId is not null");
                return db.ExecuteReader<VWPRD_Inventory>(cc.ToString()).ToArray();
            }
        }
        public Guid[] GetPRD_TitanDeviceActivatedInventoryIds(DateTime startDate, DateTime endDate)
        {
           
            using (var db = GetDB())
            {
                return db.Table<VWPRD_TitanDeviceActivated>().Where(x => x.productId_Title != null && x.CreatedOfTitan != null && x.InventoryId != null).Execute().ToList().Where(x =>  (x.CreatedOfTitan.Value.Date >= startDate.Date && x.CreatedOfTitan.Value.Date <= endDate.Date)).
                  Select(x=>x.InventoryId.Value) 
                   .ToArray();
            }
        }        
        
        public VWPRD_TitanDeviceActivated[] GetVWPRD_TitanDeviceActivatedWithDates(DateTime startDate, DateTime endDate)
        {
           
            using (var db = GetDB())
            {
                return db.Table<VWPRD_TitanDeviceActivated>().Where(x => x.productId_Title != null && x.CreatedOfTitan != null && x.InventoryId != null && x.CreatedOfTitan >= startDate && x.CreatedOfTitan <= endDate).Execute().ToArray();
            }
        }

        //test
        public PRD_TitanDeviceActivated[] GetPRD_TitanDeviceActivatedInventoryAndProductNotNull(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x =>  x.InventoryId != null && x.ProductId != null).Execute().ToArray();
            }
        }
    }
}
