using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
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
                return db.Table<PRD_TitanDeviceActivated>().Execute().Where(x=>x.InventoryId!=null && x.ProductId != null).Count();
            }
        }
        public int GetPRD_TitanDeviceActivatedTodayCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                return db.Table<PRD_TitanDeviceActivated>().Where(x => x.CreatedOfTitan > today&& x.InventoryId != null && x.ProductId != null).Execute().Count();
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
                return db.Table<VWPRD_TitanDeviceActivated>().Where(x => x.productId_Title!=null&&x.CreatedOfTitan!=null && x.InventoryId != null).Execute().ToList().Where(x=> (x.CreatedOfTitan.Value.Date >= startDate.Date && x.CreatedOfTitan.Value.Date <= endDate.Date)).
                   GroupBy(a => a.productId_Title).Select(b => new SellOutReportModel { Count = b.Count(), Name = b.Key })
                   .ToArray();
            }
        }
        public SellOutReportModel[] GetPRD_TitanDeviceActivatedSellOutDist(Guid[]ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.id.In(ids)&&x.lastActionDataCompanyId_Title!=null).Execute().ToList().GroupBy(x=>x.lastActionDataCompanyId_Title).Select(x=>new SellOutReportModel {Count=x.Count(),Name=x.Key }).ToArray();
            }
        }


        public VWPRD_Inventory[] GetPRD_TitanDeviceActivatedSellOutChartInventoryData(Guid[] ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.id.In(ids) && x.lastActionDataCompanyId_Title != null).Execute().ToArray();
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
       
    }
}
