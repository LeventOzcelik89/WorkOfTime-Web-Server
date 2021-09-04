using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_ProductionSchemaStageController : Controller
    {

        [PageInfo("Üretim Aşaması Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductionSchemaStage(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductionSchemaStageCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Üretim Aşaması Dropdown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductionSchemaStage(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Üretim Aşaması Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VWPRD_ProductionSchemaStageModel data)
        {
            return View(data.Load());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Üretim Aşaması Ekleme", SHRoles.Personel)]
        public JsonResult Insert(VWPRD_ProductionSchemaStageModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error(dbresult.message, "Başarısız....")
            };
            result.FeedBack.message = dbresult.message;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Üretim Aşaması Güncelleme", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductionSchemaStageById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Üretim Aşaması Güncelleme", SHRoles.Personel)]
        public JsonResult Update(VWPRD_ProductionSchemaStageModel item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error(dbresult.message,"Güncelleme işlemi başarısız")
            };
            result.FeedBack.message = dbresult.message;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Üretim Aşaması Silme", SHRoles.StokYoneticisi)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbresult = new VWPRD_ProductionSchemaStageModel { id = id }.Delete();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error(dbresult.message, dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Üretim Aşaması Sırası Küçültme", SHRoles.Personel)]
        public JsonResult changeStageNumber(Guid id, bool isUp)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = db.GetPRD_ProductionSchemaStageById(id);
            if (!item.stageNum.HasValue || !item.productionSchemaId.HasValue)
            {
                return Json(new ResultStatusUI { FeedBack = feedback.Warning("Sıra numarası ve bağlı olduğu şema boş olamaz") }, JsonRequestBehavior.AllowGet);
            }

            var newNum = isUp
                ? item.stageNum.Value - 1
                : item.stageNum.Value + 1;

            var beUpdate = db.GetPRD_ProductionSchemaStageByStageNumAndProductionSchemaId(newNum, item.productionSchemaId.Value);
            if (beUpdate == null)
            {
                return Json(new ResultStatusUI { FeedBack = feedback.Warning("Aşama zaten " + (isUp ? "en üst" : "en alt") + " sırada") }, JsonRequestBehavior.AllowGet);
            }

            item.stageNum = newNum;

            beUpdate.stageNum = isUp
                ? beUpdate.stageNum + 1
                : beUpdate.stageNum - 1;

            var dbresult = db.BulkUpdatePRD_ProductionSchemaStage(new PRD_ProductionSchemaStage[] { item, beUpdate });
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Sıra değiştirme işlemi başarılı") : feedback.Error(dbresult.message,"Sıra değiştirme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}
