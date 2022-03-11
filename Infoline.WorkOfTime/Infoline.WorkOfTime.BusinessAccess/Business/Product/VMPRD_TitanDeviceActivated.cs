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
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            this.db = this.db ?? new WorkOfTimeDatabase();
            var getAllTitan = db.GetPRD_TitanDeviceActivatedInventoryAndProductNotNull();
            var monthTitan = getAllTitan.Where(x => x.CreatedOfTitan > DateTime.Now.AddDays(-30));
            var sevenDayTitan = monthTitan.Where(x => x.CreatedOfTitan > DateTime.Now.AddDays(-7));
            var todayTitan = sevenDayTitan.Where(x => x.CreatedOfTitan >= today);


            var getAllCount = getAllTitan.Length;
            var getTodayCount = todayTitan.Count();
            var getSevenDayCount = sevenDayTitan.Count();
            var getMonthCount = monthTitan.Count();
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
                objects = db.GetPRD_TitanDeviceActivatedSellOutProductQuery(startDate, endDate)
            };
        }
        public ResultStatus GetProductSellOutDistReport(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_TitanDeviceActivatedSellOutDistQuery(startDate, endDate);
            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }
        public ResultStatus GetProductSellOutDistrubitorReport(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_TitanDeviceActivatedSellOutDistrubitorQuery(startDate, endDate);
            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }
        public ResultStatus GetProductSellOutProductChartData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = new ChartData();
            var getAllDevices = db.GetVWPRD_TitanDeviceActivatedWithDates(startDate, endDate);
            var getDates = getAllDevices.GroupBy(x => x.created.Value.Date).OrderBy(x => x.Key).Select(x => x.Key).ToList();
            data.Dates = getDates.Select(x => x.ToShortDateString()).ToList();
            var groupedProducts = getAllDevices.GroupBy(x => x.productId_Title).ToList();
            foreach (var devices in groupedProducts)
            {
                var model = new ChartModel();
                model.name = devices.Key;
                var groupedDevices = devices.GroupBy(x => x.created.Value.Date);
                var outcastedDates = getDates.Where(x => x.In(groupedDevices.Select(a => a.Key.Date).ToArray())).ToList();
                var outcasteds = outcastedDates.Select(x => new CountModel { Date = x.Date, Count = 0 });
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
        public ResultStatus GetProductSellOutDistChartData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = new ChartData();
            var allInventories = db.GetPRD_TitanDeviceActivatedSellOutChartInventoryDataQuery(startDate, endDate);

            var getDates = allInventories.GroupBy(x => x.created.Value.Date).Select(x => x.Key).OrderBy(x => x).ToList();
            var inventories = allInventories.GroupBy(x => x.lastActionDataCompanyId_Title);
            data.Dates = getDates.Select(x => x.ToShortDateString()).ToList();
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

        public SellerReportPage GetDistReportForSellerWithDates(Guid distId, DateTime startDate, DateTime endDate)
        {
            var db = new WorkOfTimeDatabase();

            var getData = db.GetVWPRD_EntegrationActionSellerReportByDistIdAndDates(distId, startDate, endDate);
            var sallesByGrouped = getData.GroupBy(x => x.CustomerOperatorName).Select(a => new SallesByGrouped { CustomerOperatorName = a.Key != null ? a.Key : "Bayi İsmi Bulunamadı.", activatedCount = a.Sum(s => s.activatedCount), salesCount = a.Sum(s => s.salesCount), returnSalesCount = a.Sum(s => s.returnSalesCount), notActivatedCount = a.Sum(s => s.notActivatedCount) });

            var activatedCount = sallesByGrouped.Sum(a => a.activatedCount);
            var salesCount = sallesByGrouped.Sum(a => a.salesCount);
            var returnSalesCount = sallesByGrouped.Sum(a => a.returnSalesCount);
            var notActivatedCount = sallesByGrouped.Sum(a => a.notActivatedCount);

            return new SellerReportPage
            {
                sallesByGrouped = sallesByGrouped,
                totalActivatedCount = activatedCount,
                totalSalesCount = salesCount,
                returnSalesCount= returnSalesCount,
                totalNotActivatedCount = notActivatedCount
            };
        }
        public SellOutReportModel[] GetDistReportForSeller(Guid distId)
        {
            var db = new WorkOfTimeDatabase();

            var getData = db.GetVWPRD_EntegrationActionByDistrubutorId(distId);

            var groupCustomer = getData.GroupBy(a => a.CustomerOperatorId);

            var sellOutReportModelList = new List<SellOutReportModel>();

            foreach (var item in groupCustomer)
            {
                var sellOutReportModel = new SellOutReportModel();
                sellOutReportModel.Count = getData.Where(a => a.CustomerOperatorId == item.Key).Count();
                sellOutReportModel.Id = item.Key;
                sellOutReportModel.Name = getData.Where(a => a.CustomerOperatorId == item.Key).Select(a => a.CustomerOperatorName).FirstOrDefault();
                sellOutReportModel.SellingCount = getData.Where(a => a.CustomerOperatorId == item.Key && a.ActivationDate.HasValue).Count();

                sellOutReportModelList.Add(sellOutReportModel);
            }
            return sellOutReportModelList.ToArray();
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

    public class SallesByGrouped
    {
        public string CustomerOperatorName { get; set; }
        public int activatedCount { get; set; }
        public int salesCount { get; set; }
        public int returnSalesCount { get; set; }
        public int notActivatedCount { get; set; }
    }

    public class SellerReportPage
    {
        public IEnumerable<SallesByGrouped> sallesByGrouped { get; set; }
        public int totalActivatedCount { get; set; }
        public int totalSalesCount { get; set; }
        public int returnSalesCount { get; set; }
        public int totalNotActivatedCount { get; set; }
    }

    public class PRD_EntegrastionActionSellerReport : VWPRD_EntegrationAction
    {
        public VWPRD_EntegrationAction[] entegrationActions { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public PRD_EntegrastionActionSellerReport Load()
        {
            var db = new WorkOfTimeDatabase();

            return this;
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
