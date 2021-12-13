using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess.Business.SH;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_ReportController : Controller
    {

        [PageInfo("Kayıtlı Raporlarım Listesi", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            return View(VWSH_ReportModel.GetPageModel(userStatus, "Index"));
        }

      


        [PageInfo("Kayıtlı Raporlarım Oluştur Sayfası", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Insert(VWSH_ReportModel model)
        {
            return View(model.Load());
        }

        [PageInfo("Kayıtlı Raporlarım Oluştur Method", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VWSH_ReportModel model, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            return Json(new ResultStatusUI(model.Save(userStatus.user.id)), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Kayıtlı Raporlarım Sil Metodu", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            return Json(new ResultStatusUI(new VWSH_ReportModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Kayıtlı Raporlarım Verileri Alma Metodu", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_Report(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_ReportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Kayıtlı Raporlarım Verileri Dropdown Metodu", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_Report(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Kayıtlı Rapor Kaydet&Güncelle", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public ContentResult UpsertReport(VWSH_ReportModel model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];

            var dbRes = model.Save(userStatus.user.id);

            if (dbRes.result)
            {
                return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI
                {
                    Result = dbRes.result,
                    FeedBack = new FeedBack().Success("İşlem başarıyla tamamlandı.")
                }), "application/json");
            }

            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI
            {
                Result = dbRes.result,
                FeedBack = new FeedBack().Error(null, dbRes.message)
            }), "application/json");
        }

        [PageInfo("Kayıtlı Rapor Çalıştır", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Run(Guid id)
        {

            var db = new WorkOfTimeDatabase();
            var report = db.GetSH_ReportById(id);

            switch ((EnumSH_ReportType)report.type)
            {
                case EnumSH_ReportType.userDetailReport:
                default:
                    return RedirectToAction("Report", "VWEMP_Employee", new { area = "EMP", id = id });
            }

        }
    }
}
