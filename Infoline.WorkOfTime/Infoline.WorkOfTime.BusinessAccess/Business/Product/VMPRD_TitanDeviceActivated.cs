using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    public class VMPRD_TitanDeviceActivated : VWPRD_TitanDeviceActivated
    {
        private TitanServices TitanServices = new TitanServices();
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public IGeometry Location { get; set; }
        public VMPRD_TitanDeviceActivated Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            if (this.SerialNumber != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedBySerialCode(SerialNumber) ?? new PRD_TitanDeviceActivated());
            }
            else if (this.InventoryId != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedByInventoryId(InventoryId.Value) ?? new PRD_TitanDeviceActivated());
            }
            else if (this.id != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedById(id) ?? new PRD_TitanDeviceActivated());
            }
            return this;
        }
        public IndexData GetIndexData()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var getAllCount = db.GetPRD_TitanDeviceActivatedCount();
            var getTodayCount = db.GetPRD_TitanDeviceActivatedTodayCount();
            var getSevenDayCount = db.GetPRD_TitanDeviceActivatedSevenDaysCount();
            var getMonthCount = db.GetPRD_TitanDeviceActivatedThirtyDaysCount();
            return
                new IndexData
                {
                    All = getAllCount,
                    Today = getTodayCount,
                    Seven = getSevenDayCount,
                    Month = getMonthCount
                };
        }
        public ResultStatus GetProductSellOutProductReport(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            return new ResultStatus
            {
                result = true,
                objects = db.GetPRD_TitanDeviceActivatedSellOutProduct(startDate, endDate).OrderByDescending(x=>x.Count)
            };
        }
        public ResultStatus GetProductSellOutDistReport(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getIds = db.GetPRD_TitanDeviceActivatedInventoryIds(startDate, endDate);
            var getData = db.GetPRD_TitanDeviceActivatedSellOutDist(getIds);
            return new ResultStatus
            {
                result = true,
                objects = getData.OrderByDescending(x => x.Count)
            };
        }
        public ResultStatus GetProductSellOutProductChartData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = new ChartData();
            var getAllDevices = db.GetVWPRD_TitanDeviceActivated().Where(x => x.productId_Title != null && x.CreatedOfTitan != null && x.InventoryId != null && (x.CreatedOfTitan >= startDate && x.CreatedOfTitan <= endDate)).ToList();
            var getDates = getAllDevices.GroupBy(x => x.CreatedOfTitan.Value.Date).OrderBy(x => x.Key).Select(x => x.Key).ToList();
            data.Dates = getDates.Select(x=>x.ToShortDateString()).ToList();
            var groupedProducts = getAllDevices.GroupBy(x => x.productId_Title).ToList();
            foreach (var devices in groupedProducts)
            {
                var model = new ChartModel();
                model.name = devices.Key;
                var groupedDevices = devices.GroupBy(x => x.CreatedOfTitan.Value.Date);
                var outcastedDates = getDates.Where(x => x.In(groupedDevices.Select(a => a.Key.Date).ToArray())).ToList();
                var outcasteds=outcastedDates.Select(x => new CountModel { Date = x.Date, Count = 0 });
                var existents = groupedDevices.Select(x=>new CountModel {Date=x.Key,Count=x.Count() });
                model.data= outcasteds.Concat(existents).OrderBy(x=>x.Date).Select(x=>x.Count).ToList();
                data.Series.Add(model);
            }
            
            return new ResultStatus
            {
                result = true,
                objects = new { data = data }
            };
        }
        public ResultStatus GetProductSellOutDistChartData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = new ChartData();
            var getAllDevices = db.GetVWPRD_TitanDeviceActivated().Where(x => x.productId_Title != null && x.CreatedOfTitan != null && x.InventoryId != null && (x.CreatedOfTitan >= startDate && x.CreatedOfTitan <= endDate)).ToList();
            var allInventories =db.GetPRD_TitanDeviceActivatedSellOutChartInventoryData(getAllDevices.Where(x => x.InventoryId.HasValue).Select(x=>x.InventoryId.Value).ToArray());
            var getDates = allInventories.GroupBy(x => x.created.Value.Date).Select(x=>x.Key).OrderBy(x=>x).ToList();
            var inventories = allInventories.GroupBy(x => x.lastActionDataCompanyId_Title);
            data.Dates = getDates.Select(x => x.ToShortDateString()).ToList() ;
            foreach (var devices in inventories)
            {
                var model = new ChartModel();
                model.name = devices.Key;
                var groupedDevices = devices.GroupBy(x => x.created.Value.Date);
                var outcastedDates = getDates.Where(x => x.In(groupedDevices.Select(a => a.Key.Date).ToArray())).ToList();
                var outcasteds = outcastedDates.Select(x => new CountModel { Date = x, Count = 0 });
                var existents = groupedDevices.Select(x => new CountModel { Date = x.Key, Count = x.Count() });
                model.data = outcasteds.Concat(existents).OrderBy(x => x.Date).Select(x => x.Count).ToList();
                data.Series.Add(model);
            }
            return new ResultStatus
            {
                result = true,
                objects = new { data = data }
            };
        }
        public ResultStatus GetAllDevices()
        {
            return TitanServices.GetAllDevices();
        }
        public ResultStatus GetDeviceById(Guid id)
        {
            return TitanServices.GetDeviceById(id);
        }
        public ResultStatus GetDeviceInformation(Guid id)
        {
            return TitanServices.GetDeviceInformation(id);
        }
        public ResultStatus GetDeviceActivationInformation()
        {
            return TitanServices.GetDeviceActivationInformations();
        }
    }
    public class IndexData
    {
        public int All { get; set; }
        public int Today { get; set; }
        public int Seven { get; set; }
        public int Month { get; set; }
    }
    public class ChartModel
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }
    public class ChartData
    {
        public List<ChartModel> Series { get; set; } = new List<ChartModel>();
        public List<string> Dates { get; set; } = new List<string>();
    }
    public class CountModel
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
