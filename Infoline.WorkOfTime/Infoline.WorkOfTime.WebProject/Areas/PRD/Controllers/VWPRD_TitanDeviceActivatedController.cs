using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Business.Product;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class SellOutDashboardModel
    {
        public IndexData IndexData { get; set; }
        public ResultStatus ProductSellOut { get; set; }
        public ResultStatus DistSellOut { get; set; }
        public ResultStatus ProductSellOutProductChartData { get; set; }
        public ResultStatus ProductSellOutDistChartData { get; set; }

    }
    public class VWPRD_TitanDeviceActivatedController : Controller
    {
        [PageInfo("Titan Cihaz Listeleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator  )]
        public ActionResult Index()
        {
            ViewBag.data = new VMPRD_TitanDeviceActivated().GetIndexData();
            return View();
        }
        [PageInfo("Titan Cihaz Listeleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult DetailForInventory(VMPRD_TitanDeviceActivated model)
        {
            var data = model.Load();
            return View(data);
        }
        [PageInfo("Titan Cihaz Sell Out Raporu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici,SHRoles.SistemYonetici,SHRoles.UretimYonetici)]
        public ActionResult SellOutDashboard()
        {
            return View();
        }
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var dataValues = db.GetVWPRD_TitanDeviceActivated(condition);
            dataValues.Each(x => x.id = (Guid)(x.InventoryId.HasValue == true ? x.InventoryId : new Guid()));
            var data = dataValues.RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_TitanDeviceActivatedCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Titan Tüm Cihazları Alma Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetAllDevices()
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetAllDevices(), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihazları Detaylı Bilgi Alma Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetDeviceActivationInformation()
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceActivationInformation(), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihazları Detaylı Bilgi Alma Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetDeviceById(Guid id)
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceById(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeviceInformation(Guid id)
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceInformation(id), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetIndexData()
        {
            return Json(new VMPRD_TitanDeviceActivated().GetIndexData(), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetProductSellOut(DateTime startDate, DateTime endDate)
        {
            var data = new VMPRD_TitanDeviceActivated().GetProductSellOutProductReport(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetDistSellOut(DateTime startDate, DateTime endDate)
        {
            var data = new VMPRD_TitanDeviceActivated().GetProductSellOutDistReport(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetProductSellOutProductChartData(DateTime startDate, DateTime endDate)
        {
            var data = new VMPRD_TitanDeviceActivated().GetProductSellOutProductChartData(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetProductSellOutDistChartData(DateTime startDate, DateTime endDate)
        {
            var data = new VMPRD_TitanDeviceActivated().GetProductSellOutDistChartData(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetSellerReport(Guid distId)
        {
            var data = new VMPRD_TitanDeviceActivated().GetDistReportForSeller(distId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetSellerReportWithDates(Guid distId, DateTime startDate, DateTime endDate)
        {
            var data = new VMPRD_TitanDeviceActivated().GetDistReportForSellerWithDates(distId,startDate,endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }        
        
        [AllowEveryone]
        [PageInfo("Dashboard Sayfası Verileri")]
        public  JsonResult GetPageReport(DateTime startDate, DateTime endDate)
        {
            SellOutDashboardModel pageReport = new SellOutDashboardModel();
            pageReport.IndexData = new VMPRD_TitanDeviceActivated().GetIndexData();
            pageReport.ProductSellOut = new VMPRD_TitanDeviceActivated().GetProductSellOutProductReport(startDate, endDate);
            pageReport.DistSellOut = new VMPRD_TitanDeviceActivated().GetProductSellOutDistReport(startDate, endDate);
            pageReport.ProductSellOutProductChartData = new VMPRD_TitanDeviceActivated().GetProductSellOutProductChartData(startDate, endDate);
            pageReport.ProductSellOutDistChartData = new VMPRD_TitanDeviceActivated().GetProductSellOutDistChartData(startDate, endDate);

            return Json(pageReport, JsonRequestBehavior.AllowGet);
        }
    }
}
