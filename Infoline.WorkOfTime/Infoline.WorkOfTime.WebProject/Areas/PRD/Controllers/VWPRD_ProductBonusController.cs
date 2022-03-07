using System.Linq;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_ProductBonusController : Controller
    {
        [PageInfo("Prim Kuralları Liste", SHRoles.SistemYonetici)]
        public ViewResult Index()
        {
            return View();
        }
        [PageInfo("Veri Kaynağı",SHRoles.SistemYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductBonus(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductBonusCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Veri Kaynağı Dropdown", SHRoles.SistemYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductBonus(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Prim Kuralı Detay", SHRoles.SistemYonetici)]
        public ActionResult Detail(VMPRD_ProductBonusModel item)
        {
            var data = item.Load();
            return View(data);
        }
        [PageInfo("Prim Kuralı Ekle", SHRoles.SistemYonetici)]
        public ActionResult Insert(VMPRD_ProductBonusModel item)
        {
            var data = item.Load();
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Prim Kuralı Ekle", SHRoles.SistemYonetici)]
        public JsonResult Insert(VMPRD_ProductBonusModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Prim Tanımlama İşlemi Başarılı") : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Prim Kuralı Güncelle", SHRoles.SistemYonetici)]
        public ActionResult Update(VMPRD_ProductBonusModel item)
        {
            var data = item.Load();
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Prim Kuralı Güncelle", SHRoles.SistemYonetici)]
        public JsonResult Update(VMPRD_ProductBonusModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message, false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [PageInfo("Prim Kuralı Sİl", SHRoles.SistemYonetici)]
        public JsonResult Delete(Guid[] id)
        {
            var db = new WorkOfTimeDatabase();
            var trans = db.BeginTransaction();
            var feedback = new FeedBack();
            var result = new ResultStatus { result = true };
            
            var model = new VMPRD_ProductBonusModel();
            foreach (var i in id)
            {
                model.id = i;
                result &= model.Delete(trans);
            }
            if (result.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            var result1 = new ResultStatusUI
            {
                Result = result.result,
                FeedBack = result.result ? feedback.Success("Prim Kuralı Silme İşlemi Başarıyla Tamamlandı") : feedback.Warning(result.message)
            };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }
    }

}
