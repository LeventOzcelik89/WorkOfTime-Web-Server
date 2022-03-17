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
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_ProductProgressPaymentImportController : Controller
    {
        [PageInfo("Bayi Hakediş Listesi", SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Veri Kaynağı", SHRoles.SistemYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductProgressPaymentImport(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductProgressPaymentImportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Veri Kaynağı Dropdown", SHRoles.SistemYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductProgressPaymentImport(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Hakediş Oluştur", SHRoles.SistemYonetici)]
        public ActionResult Insert(VMPRD_ProductProgressPaymentImportModel model)
        {
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Hakediş Oluştur", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMPRD_ProductProgressPaymentImportModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Hakediş Tanımlama İşlemi Başarılı") : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Excel Hakediş Oluştur", SHRoles.SistemYonetici)]
        [HttpPost]
        public JsonResult Import(string model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var result = VMPRD_ProductProgressPaymentImportModel.Import(model, userStatus.user.id);
            return Json(new ResultStatusUI(result), JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Hakediş Güncelle", SHRoles.SistemYonetici)]
        public ActionResult Update(VMPRD_ProductProgressPaymentImportModel item)
        {
            var data = item.Load();
            return View(data);
        }

        [PageInfo("Hakediş Güncelle", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMPRD_ProductProgressPaymentImportModel item, bool? isPost)
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

        [PageInfo("Hakediş Sil", SHRoles.SistemYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            return Json(new ResultStatusUI(new VMPRD_ProductProgressPaymentImportModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
        }
    }
}
