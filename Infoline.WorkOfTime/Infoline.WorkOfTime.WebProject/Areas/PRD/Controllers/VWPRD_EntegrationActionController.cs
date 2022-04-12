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

        [PageInfo("Titan Cihaz Sell Out Raporu", SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli,SHRoles.SistemYonetici)]
        public ActionResult SellOutReport()
        {
            return View();
        }

        [PageInfo("Titan Cihaz Sell Out Raporu", SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli, SHRoles.SistemYonetici)]
        public ActionResult SellOutReportDetail(VMPRD_EntegrationActionModel item,DateTime startDate, DateTime endDate)
        {
            item.startDate = startDate;
            item.endDate = endDate;
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
        public JsonResult GetPageReportDetail(DateTime startDate, DateTime endDate,Guid DistrubitorId)
        {
            VMPRD_EntegrationActionModel pageReport = new VMPRD_EntegrationActionModel();
            var t1 = Task.Run(() =>
            {
                pageReport.DistData = new VMPRD_EntegrationActionModel().SellOutProductDetailDistData(startDate, endDate, DistrubitorId);
            });
            var t2 = Task.Run(() =>
            {
                pageReport.DealarDetailData = new VMPRD_EntegrationActionModel().SellOutProductDealarData(startDate, endDate, DistrubitorId);
            });
            var t3 = Task.Run(() =>
            {
                pageReport.DealarAndProductData= new VMPRD_EntegrationActionModel().SellOutProductDealarProductData(startDate, endDate, DistrubitorId);
            });

            t1.Wait();
            t2.Wait();
            t3.Wait();
            return Json(pageReport, JsonRequestBehavior.AllowGet);
        }
    }
}
