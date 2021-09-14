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
        public JsonResult Insert(VMPRD_CompanyBasedPriceDetailModel item)
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
                    FeedBack = feedback.Success("Kaydetme  işlemi tamamlandı", false, Url.Action("Index", "VWPRD_CompanyBasedPrice", new { area = "PRD" }))
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning(dbresult.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowEveryone]
        public ActionResult Update(Guid id)
        {
            var data = new VMPRD_CompanyBasedPriceDetailModel { companyBasedPriceId = id };
            return View(data.Load());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [AllowEveryone]
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, VWPRD_CompanyBasedPriceDetailDto item)
        {
            var model = new VMPRD_CompanyBasedPriceDetailModel().B_EntityDataCopyForMaterial(item);
            var userStatus = (PageSecurity)Session["userStatus"];
            return Json(model.Save(), JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        public ActionResult GetVWCompanyBasedPriceDetailByCompanyBasedPriceId(Guid id, [DataSourceRequest] DataSourceRequest request)
        {
            var VWCompanyBasedPriceDetailList = new VMPRD_CompanyBasedPriceDetailModel().GetVWCompanyBasedPriceDetailByCompanyBasedPriceId(id).ToDataSourceResult(request);
            return Json(VWCompanyBasedPriceDetailList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [AllowEveryone]
        public ActionResult DeleteInline([DataSourceRequest] DataSourceRequest request, VWPRD_CompanyBasedPriceDetailDto item)
        {
            var model = new VMPRD_CompanyBasedPriceDetailModel().B_EntityDataCopyForMaterial(item);
            var rs = model.Delete();
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [AllowEveryone]
        public ActionResult UpdateInline([DataSourceRequest] DataSourceRequest request, VWPRD_CompanyBasedPriceDetailDto item)
        {
            var model = new VMPRD_CompanyBasedPriceDetailModel().B_EntityDataCopyForMaterial(item);
            var rs = model.UpdateInline();


            var feedback = new FeedBack();
            if (rs.result == true)
            {
                var result = new ResultStatusUI
                {
                    Result = rs.result,
                    FeedBack = rs.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning(rs.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = rs.result,
                    FeedBack = rs.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning(rs.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [AllowEveryone]
        public ActionResult InsertInline([DataSourceRequest] DataSourceRequest request, VWPRD_CompanyBasedPriceDetailDto item)
        {
            var model = new VMPRD_CompanyBasedPriceDetailModel().B_EntityDataCopyForMaterial(item);
            var rs = model.InsertInline();
            var feedback = new FeedBack();
            if (rs.result == true)
            {
                var result = new ResultStatusUI
                {
                    Result = rs.result,
                    FeedBack = rs.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning(rs.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = rs.result,
                    FeedBack = rs.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning(rs.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
