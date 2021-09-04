using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HDM.Controllers
{
	public class VWHDM_SuggestionController : Controller
    {
        [PageInfo("Çözüm Önerisi Ekleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Index()
        {
            return View();
        }


        [PageInfo("Çözüm Önerileri Metodu", SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep, SHRoles.YardimMasaPersonel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var suggestions = db.GetVWHDM_Suggestion(condition);

            foreach (var suggestion in suggestions)
            {
                suggestion.content = null;
            }

            var data = suggestions.RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHDM_SuggestionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Çözüm Önerileri DropDown Metodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var suggestions = db.GetVWHDM_Suggestion(condition);

            foreach (var suggestion in suggestions)
            {
                suggestion.content = null;
            }

            return Content(Infoline.Helper.Json.Serialize(suggestions), "application/json");
        }

        [PageInfo("Çözüm Önerisi Ekleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Insert(VMHDM_SuggestionModel item)
        {
            item.Load();
            return View(item);
        }


        [PageInfo("Çözüm Önerisi Ekleme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Insert(VMHDM_SuggestionModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Çözüm Önerisi Düzenleme Sayfası", SHRoles.YardimMasaYonetim)]
        public ActionResult Update(Guid id)
        {
            var data = new VMHDM_SuggestionModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Çözüm Önerisi Düzenleme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Update(VMHDM_SuggestionModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Öneri Dosyası Ekleme", SHRoles.YardimMasaYonetim)]
        public JsonResult EditorUploadFile(HttpPostedFileBase upload, string id)
        {
            if (upload != null)
            {
                var rs = new LocalFileProvider("HDM_Suggestion").Import(new Guid(id), "Öneri Dosyası", upload);
                var returnObj = new
                {
                    uploaded = 1,
                    fileName = ((Dictionary<string, object>)rs.Object)["name"],
                    url = ((Dictionary<string, object>)rs.Object)["url"],
                };
                return Json(returnObj, JsonRequestBehavior.AllowGet);
            }
         
            return Json(new { uploaded = 0, error = new { message = "İşlem Sırasında Hata Meydana Geldi" } }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Çözüm Önerisi Silme Metodu", SHRoles.YardimMasaYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var item = new VMHDM_SuggestionModel { id = id }.Load();
            var dbresult = item.Delete();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız." + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
