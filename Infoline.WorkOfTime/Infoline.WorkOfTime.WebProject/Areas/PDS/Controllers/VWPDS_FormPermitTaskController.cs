using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS.Controllers
{
	public class VWPDS_FormPermitTaskController : Controller
    {
        [PageInfo("Form Değerlendirme Görevi Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var item = new VMPDS_FormPermitTaskModel { id = id }.Load();
            return View(item);
        }

        [PageInfo("Form Değerlendirme Görevi Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(VMPDS_FormPermitTaskModel item)
        {
            item.Load();
            return View(item);
        }

        [HttpPost]
        [PageInfo("Form Değerlendirme Görevi Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(VMPDS_FormPermitTaskModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbres = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result == true ? feedback.Success("Görev kaydetme işlemi başarılı.") : feedback.Warning("Kaydetme işlemi başarısız.Mesaj : " + dbres.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Form Değerlendirme Görevi Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var item = new VMPDS_FormPermitTaskModel { id = id }.Load();
            return View(item);
        }

        [HttpPost]
        [PageInfo("Form Değerlendirme Görevi Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(VMPDS_FormPermitTaskModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbres = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result == true ? feedback.Success("Form değerlendirme aralığı düzenleme işlemi başarılı.") : feedback.Warning("Düzenleme işlemi başarısız.Mesaj : " + dbres.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Form Değerlendirme Görevi Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var item = new VMPDS_FormPermitTaskModel { id = id }.Load();
            var dbresult = item.Delete();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Form Değerlendirme Görevi Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormPermitTask(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPDS_FormPermitTaskCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Form Değerlendirme Görevi Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormPermitTask(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [HttpPost]
        [PageInfo("Form Değerlendirme Görevi Aktif-Pasif", SHRoles.IKYonetici)]
        public JsonResult MakeActivePassive(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = db.GetPDS_FormPermitTaskById(id);

            item.status = !item.status;

            var dbresult = db.UpdatePDS_FormPermitTask(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
