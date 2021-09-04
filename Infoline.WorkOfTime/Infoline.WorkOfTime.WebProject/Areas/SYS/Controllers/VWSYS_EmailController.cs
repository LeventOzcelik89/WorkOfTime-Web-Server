using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class VWSYS_EmailController : Controller
    {
        [PageInfo("Email Logları",SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Email Logları Methodu", SHRoles.SistemYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSYS_Email(condition).ToDataSourceResult(request);
            data.Total = db.GetVWSYS_EmailCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Email Sayısı DataSource", SHRoles.SistemYonetici)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWSYS_EmailCount(condition.Filter);
            return count;
        }

        [PageInfo("Email Logları Detayı", SHRoles.SistemYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSYS_EmailById(id);
            return View(data);
        }

        [PageInfo("Email Logu Silme", SHRoles.SistemYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = id.Select(a => new SYS_Email { id = new Guid(a) });
            var dbresult = db.BulkDeleteSYS_Email(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
