using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HDM.Controllers
{
	public class VWHDM_TicketMessageController : Controller
    {
        [PageInfo("Yardım Talebi Mesajları Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_TicketMessage(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHDM_TicketMessageCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Yardım Talebi Mesaj Detayı", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        public ActionResult Detail(Guid id)
        {
            var data = new VMHDM_TicketMessageModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Yardım Talebi Mesaj Ekleme Sayfası", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        public ActionResult Insert(VMHDM_TicketMessageModel item)
        {
            item.Load();
            return View(item);
        }

        [PageInfo("Yardım Talebi Mesaj Ekleme Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        [HttpPost, ValidateInput(false)]
        public JsonResult Insert(VMHDM_TicketMessageModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("İşlem başarıyla gerçekleşti") : feedback.Warning("İşlem gerçekleştirilirken bir sorun oluştu. " + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Yardım Talebi Düzenleme Sayfası", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        public ActionResult Update(Guid id)
        {
            var data = new VMHDM_TicketMessageModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Yardım Talebi Düzenleme Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Update(VMHDM_TicketMessageModel item)
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

        [PageInfo("Mesaj Dosyası Ekleme", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        public JsonResult EditorUploadFile(HttpPostedFileBase upload, string id)
        {
            if (upload != null)
            {
                var rs = new LocalFileProvider("HDM_TicketMessage").Import(new Guid(id), "Ek Mesaj Dosyası", upload);
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

        [PageInfo("Yardım Talebi Mesajı Sil", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = id.Select(a => new HDM_TicketMessage { id = new Guid(a) });
            var dbresult = db.BulkDeleteHDM_TicketMessage(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
