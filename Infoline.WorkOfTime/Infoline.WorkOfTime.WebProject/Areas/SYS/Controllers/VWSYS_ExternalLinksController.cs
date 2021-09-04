using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class VWSYS_ExternalLinksController : Controller
    {
        [PageInfo("Dış Bağlantı Yönetimi", SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Dış Bağlntılar Veri Methodu", SHRoles.SistemYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSYS_ExternalLinks(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSYS_ExternalLinksCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Dış Bağlntılar Dropdown Veri Methodu", SHRoles.SistemYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSYS_ExternalLinks(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Dış Bağlntılar Ekleme", SHRoles.SistemYonetici)]
        public ActionResult Insert()
        {
            var data = new VWSYS_ExternalLinks { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("Dış Bağlntılar Ekleme Methodu", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SYS_ExternalLinks item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();


            var control = db.GetSYS_ExternalLinksByUrl(item.Url);
            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı bağlantı Url'i daha önce sisteme eklenmiş.")
                }, JsonRequestBehavior.AllowGet);
            }


            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertSYS_ExternalLinks(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Dış bağlantı ekleme işlemi gerçekleştirildi.") : feedback.Warning("Dış bağlantı ekleme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Dış Bağlntılar Güncelleme", SHRoles.SistemYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSYS_ExternalLinksById(id);
            return View(data);
        }

        [PageInfo("Dış Bağlntıların güncelleme işleminin gerçekleştirildiği methoddur.", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SYS_ExternalLinks item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var control = db.GetSYS_ExternalLinksByUrlAndId(item.id, item.Url);
            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı bağlantı Url'i ile daha önce kayıt gerçekleştirilmiş.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateSYS_ExternalLinks(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Dış bağlantı güncelleme işlemi gerçekleştirildi.") : feedback.Warning("Dış bağlantı güncelleme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Dış Bağlntıların silme işleminin gerçekleştirildiği methoddur.", SHRoles.SistemYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new SYS_ExternalLinks { id = new Guid(a) });

            var dbresult = db.BulkDeleteSYS_ExternalLinks(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Dış bağlantı silme işlemi başarı ile gerçekleşti.") : feedback.Error("Dış bağlantı silme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
