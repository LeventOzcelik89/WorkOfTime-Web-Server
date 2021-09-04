using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.CSM.Controllers
{
	public class VWCSM_StageController : Controller
    {
        [PageInfo("Süreç Aşamaları", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Index()
        {
            return View();
        }


        [PageInfo("Aşamalar Metodu", SHRoles.AdayOgrenciYonetim)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_Stage(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCSM_StageCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Aşamalar Dropdown Methodu", SHRoles.Personel, SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_Stage(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Aşama Detayı", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_StageById(id);
            return View(data);
        }


        [PageInfo("Aşama Ekleme Sayfası", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Insert()
        {
            var data = new VWCSM_Stage { id = Guid.NewGuid() };
            return View(data);
        }


        [PageInfo("Aşama Ekleme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CSM_Stage item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertCSM_Stage(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Aşama Düzenleme Sayfası", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_StageById(id);
            return View(data);
        }


        [PageInfo("Aşama Düzenleme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CSM_Stage item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateCSM_Stage(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Aşama Silme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var activityControl = db.GetCSM_ActivityByStageId(id);

            if (activityControl.Length > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Bu aşamaya sahip aktivite olduğu için silinemez.")
                }, JsonRequestBehavior.AllowGet);
            }

            var stage = db.GetCSM_StageById(id);
            var dbresult = db.DeleteCSM_Stage(stage);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
