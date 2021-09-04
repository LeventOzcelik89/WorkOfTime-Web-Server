using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS.Controllers
{
	public class VWPDS_EvaluateFormController : Controller
    {
        [PageInfo("Değerlendirme Formları", SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Performans Formu Ekleme (Yeni)", SHRoles.IKYonetici)]
        public ActionResult Insert(VMPDS_EvaluateFormModel item)
        {
            item.Load();
            if (item.created != null)
            {
                item.id = Guid.NewGuid();
                item.formName = !String.IsNullOrEmpty(item.formName) ? item.formName + " (Kopya)" : item.formName;
                item.status = true;
            }

            return View(item);
        }

        [HttpPost]
        [PageInfo("Performans Formu Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(VMPDS_EvaluateFormModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbres = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result == true ? feedback.Success("Form kaydetme işlemi başarılı.") : feedback.Error("Form kaydetme işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Performans Formu Düzenleme (Yeni)", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var item = new VMPDS_EvaluateFormModel { id = id }.Load();
            return View(item);
        }

        [HttpPost]
        [PageInfo("Performans Formu Düzenleme", SHRoles.IKYonetici)]
        public JsonResult Update(VMPDS_EvaluateFormModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbres = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result == true ? feedback.Success("Form düzenleme işlemi başarılı.") : feedback.Error("Form düzenleme işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Performans Formu Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var item = new VMPDS_EvaluateFormModel { id = id }.Load();
            return View(item);
        }

        [HttpPost]
        [PageInfo("Performans Formu Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var dbresult = new VMPDS_EvaluateFormModel { id = id }.Load().Delete();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Performans Formları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_EvaluateForm(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPDS_EvaluateFormCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Form İşlemleri Miktar DataSource", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWPDS_EvaluateFormCount(condition.Filter);
            return count;
        }

        [PageInfo("Performans Formları Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_EvaluateForm(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [HttpPost]
        [PageInfo("Performans Aktif Pasif Methodu", SHRoles.IKYonetici)]
        public JsonResult MakeActivePassive(Guid id)
        {
            var feedback = new FeedBack();
            var dbresult = new VMPDS_EvaluateFormModel { id = id }.Load().MakeActivePassive();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız. Mesaj : " + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
