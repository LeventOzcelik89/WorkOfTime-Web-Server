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
using Infoline.WorkOfTime.BusinessAccess.Business.Product;
using Infoline.WorkOfTime.BusinessData.Specific;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_CompanyBasedPriceDetailController : Controller
    {
        [AllowEveryone]
        public ActionResult Index()
        {
            return View();
        }
        [AllowEveryone]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPriceDetail(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_CompanyBasedPriceDetailCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [AllowEveryone]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPriceDetail(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [AllowEveryone]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPriceDetailById(id);
            return View(data);
        }
        [AllowEveryone]
        public ActionResult Insert()
        {
            var data = new VMPRD_CompanyBasedPriceDetailModel { id = Guid.NewGuid(), companyBasedPriceId = Guid.NewGuid() };
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [AllowEveryone]
        public JsonResult Insert(VMPRD_CompanyBasedPriceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.createdby = userStatus.user.id;
            item.created = DateTime.Now;
            var dbresult = item.Save();
            var feedback = new FeedBack();
            if (dbresult.result != false)
            {
                var result = new ResultStatusUI()
                {
                    Result = dbresult.result,
                    FeedBack = feedback.Success("Özel Fiyat Belirleme İşlemi Tamamlandı", false, Url.Action("Index", "VWPRD_CompanyBasedPrice", new { area = "PRD" }))
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Özel Fiyat Belirleme İşlemi Başarılı") : feedback.Warning(dbresult.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowEveryone]
        public ActionResult Update(Guid id)
        {
            var data = new VMPRD_CompanyBasedPriceModel { id = id };
            return View(data.Load());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [AllowEveryone]
        public JsonResult Update(VMPRD_CompanyBasedPriceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.createdby = userStatus.user.id;
            item.created = DateTime.Now;
            var dbresult = item.Save();
            var feedback = new FeedBack();
            if (dbresult.result != false)
            {
                var result = new ResultStatusUI()
                {
                    Result = dbresult.result,
                    FeedBack = feedback.Success("Özel Fiyat Belirleme Güncelleme  İşlemi Tamamlandı", false, Url.Action("Index", "VWPRD_CompanyBasedPrice", new { area = "PRD" }))
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Özel Fiyat Belirleme Güncelleme İşlemi Başarılı") : feedback.Warning(dbresult.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
