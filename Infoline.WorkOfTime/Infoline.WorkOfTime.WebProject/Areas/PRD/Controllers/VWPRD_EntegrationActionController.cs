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
    public class VWPRD_EntegrationActionController : Controller
    {
        [AllowEveryone]
        [PageInfo("Bayi Aktivasyon Raporu Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SistemYonetici, SHRoles.UretimYonetici)]
        public ActionResult SellerReport(PRD_EntegrastionActionSellerReport model, string startDate = null, string endDate = null)
        {
            if (endDate != null)
            {
                model.endDate = DateTime.Parse(endDate);
            }
            if (startDate != null)
            {
                model.startDate = DateTime.Parse(startDate);
            }
            return View(model.Load());
        }

        [PageInfo("Sell Out Raporu", SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli, SHRoles.SistemYonetici)]
        public ActionResult SellOutReport()
        {
            return View();
        }

        [PageInfo("Sell Out Raporu Detayı", SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli, SHRoles.SistemYonetici)]
        public ActionResult SellOutReportDetail(VMPRD_EntegrationActionModel item, string startDate, string endDate)
        {
            var start = Date.Parse(startDate);
            var end = Date.Parse(endDate);
            item.startDate = start;
            item.endDate = end;
            var data = item.Load();
            return View(data);
        }

        [PageInfo("Distributör Stok Raporu", SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli, SHRoles.SistemYonetici)]
        public ActionResult DistStockReport()
        {
            return View();
        }

        [PageInfo("Distributör Stok Raporu Detayı", SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli, SHRoles.SistemYonetici)]
        public ActionResult DistStockReportDetail(VMPRD_EntegrationActionModel item)
        {
            var data = item.Load();
            return View(data);
        }

        [AllowEveryone]
        [PageInfo("Bayi Aktivasyon Raporu DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SistemYonetici, SHRoles.UretimYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_EntegrationAction(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_EntegrationActionCount(condition.Filter);

            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Dashboard Sayfası Verileri")]
        public JsonResult GetPageReport(DateTime startDate, DateTime endDate)
        {
            VMPRD_EntegrationActionModel pageReport = new VMPRD_EntegrationActionModel();
            var t1 = Task.Run(() =>
            {
                pageReport.DistDataSourceReport = new VMPRD_EntegrationActionModel().SellOutReportData(startDate, endDate);
            });

            var t2 = Task.Run(() =>
            {
                pageReport.DistAndProductDataSourceReport = new VMPRD_EntegrationActionModel().SellOutProductReportData(startDate, endDate);
            });

            t1.Wait();
            t2.Wait();
            return Json(pageReport, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        [PageInfo("Dashboard Sayfası Verileri")]
        public JsonResult GetPageReportDetail(DateTime? startDate, DateTime? endDate, Guid DistrubitorId)
        {
            VMPRD_EntegrationActionModel pageReport = new VMPRD_EntegrationActionModel();
            var t1 = Task.Run(() =>
            {
                pageReport.DistData = new VMPRD_EntegrationActionModel().SellOutProductDetailDistData(startDate.Value, endDate.Value, DistrubitorId);
            });
            var t2 = Task.Run(() =>
            {
                pageReport.DealarDetailData = new VMPRD_EntegrationActionModel().SellOutProductDealarData(startDate.Value, endDate.Value, DistrubitorId);
            });
            var t3 = Task.Run(() =>
            {
                pageReport.DealarAndProductData = new VMPRD_EntegrationActionModel().SellOutProductDealarProductData(startDate.Value, endDate.Value, DistrubitorId);
            });

            t1.Wait();
            t2.Wait();
            t3.Wait();
            return Json(pageReport, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        [PageInfo("Stok Rapor Verileri")]
        public JsonResult GetPageStockReport()
        {
            VMPRD_EntegrationActionModel pageReport = new VMPRD_EntegrationActionModel();
            var t1 = Task.Run(() =>
            {
                pageReport.DistStockData = new VMPRD_EntegrationActionModel().DistStockReportData();
            });
            var t2 = Task.Run(() =>
            {
                pageReport.DistProductStockData = new VMPRD_EntegrationActionModel().DistProductStockReportData();
            });
            t1.Wait();
            t2.Wait();
            return Json(pageReport, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        [PageInfo("Stok Rapor Verileri")]
        public JsonResult GetPageStockReportDetail(Guid DistrubitorId)
        {
            VMPRD_EntegrationActionModel pageReport = new VMPRD_EntegrationActionModel();
            var t1 = Task.Run(() =>
            {
                pageReport.DistStockDetailData = new VMPRD_EntegrationActionModel().DistStockDetailDistData(DistrubitorId);
            });
            var t2 = Task.Run(() =>
            {
                pageReport.DealarStockDetailData = new VMPRD_EntegrationActionModel().DistStockProductDealarData(DistrubitorId);
            });
            var t3 = Task.Run(() =>
            {
                pageReport.DealarProductStockDetailData = new VMPRD_EntegrationActionModel().DistStockProductDealarProductData(DistrubitorId);
            });
            t1.Wait();
            t2.Wait();
            t3.Wait();
            return Json(pageReport, JsonRequestBehavior.AllowGet);
        }
    }
}
