using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HDM.Controllers
{
	public class VWHDM_IssueUserController : Controller
    {
        [PageInfo("Konuların Görevlileri Metodu", SHRoles.Personel, SHRoles.YardimMasaMusteri)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_IssueUser(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHDM_IssueUserCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Sorumlu Yönetici Ekleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Insert(VWHDM_IssueUser item)
        {
            return View(item);
        }

        [PageInfo("Sorumlu Yönetici Ekleme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(HDM_IssueUser item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertHDM_IssueUser(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Sorumlu Yönetici Düzenleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_IssueUserById(id);
            return View(data);
        }

        [PageInfo("Sorumlu Yönetici Düzenleme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(HDM_IssueUser item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateHDM_IssueUser(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Sorumlu Yönetici Silme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var issueUser = db.GetHDM_IssueUserById(id);
            var dbresult = db.DeleteHDM_IssueUser(issueUser);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
