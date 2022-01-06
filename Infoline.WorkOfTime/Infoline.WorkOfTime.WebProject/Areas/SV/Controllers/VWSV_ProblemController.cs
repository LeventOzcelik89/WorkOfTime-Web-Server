using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
    public class VWSV_ProblemController : Controller
    {
        [PageInfo("Cihaz Problemlerinin Listelendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Cihaz Problemlerinin Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_Problem(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSV_ProblemCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Cihaz Problemlerinin Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_Problem(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Cihaz Problemlerinin Detaylarının Gösterildiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Detail(VMSV_ProblemModel model)
        {

            return View(model.Load());
        }


        [PageInfo("Cihaz Problemlerinin Eklendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Insert(VMSV_ProblemModel model)
        {
            model.code = Extensions.VersionCode;
            return View(model);
        }


        [PageInfo("Cihaz Problemlerinin Eklendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMSV_ProblemModel model, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = model.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Yeni Problem oluşturma işlemi başarılı") : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [PageInfo("Cihaz Problemlerinin Güncellendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Update(Guid id)
        {
            var model = new VMSV_ProblemModel { id = id };
            var data = model.Load();
            return View(data);
        }


        [PageInfo("Cihaz Problemlerinin Güncellendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMSV_ProblemModel model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = model.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Yeni Problem Güncelleme işlemi başarılı") : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Cihaz Problemlerinin Silindiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new SV_Problem { id = new Guid(a) });

            var dbresult = db.BulkDeleteSV_Problem(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}
