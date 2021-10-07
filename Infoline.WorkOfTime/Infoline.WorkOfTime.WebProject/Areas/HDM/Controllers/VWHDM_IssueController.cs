using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HDM.Controllers
{
	public class VWHDM_IssueController : Controller
    {
        [PageInfo("Konu ve Öneriler", SHRoles.YardimMasaYonetim)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Sıkça Sorulan Sorular", SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep, SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri)]
        public ActionResult Help()
        {
            var db = new WorkOfTimeDatabase();
            var issues = db.GetVWHDM_Issue();
            var suggestions = db.GetVWHDM_Suggestion();

            var model = new VMHDM_IssueSuggestionModel
            {
                Issues = issues,
                Suggestions = suggestions
            };

            return View(model);
        }

        [PageInfo("Konu ve Sorular Metodu", SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep, SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_Issue(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHDM_IssueCount(condition.Filter);
            return Content(Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Konu ve Sorular DropDown Metodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_Issue(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Konu ve Soru Detayı", SHRoles.YardimMasaYonetim)]
        public ActionResult Detail(Guid id)
        {
            var data = new VMHDM_IssueModel { id = id }.Load();
            data.path = data.GetPath(id);
            return View(data);
        }

        [PageInfo("Konu ve Soru Ekleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Insert(VMHDM_IssueModel item)
        {
            var model = item.Load();
            return View(model);
        }

        [PageInfo("Konu ve Soru Ekleme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMHDM_IssueModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız." + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Konu ve Soru Güncelleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Update(Guid id)
        {
            var data = new VMHDM_IssueModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Konu ve Soru Güncelleme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMHDM_IssueModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız." + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Konu ve Soru Silme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var item = new VMHDM_IssueModel { id = id }.Load();
            var dbresult = item.Delete(userStatus.AuthorizedRoles);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Sorumlu yöneticiyi silme işlemi başarı ile gerçekleşti.") : feedback.Warning("Sorumlu yönetici silme işlemi başarısız oldu." + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
