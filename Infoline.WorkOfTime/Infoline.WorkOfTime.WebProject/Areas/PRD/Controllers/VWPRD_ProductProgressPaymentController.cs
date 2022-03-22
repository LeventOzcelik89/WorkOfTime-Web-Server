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
    public class VWPRD_ProductProgressPaymentController : Controller
    {
        [PageInfo("Bayi Satış Onay Listesi", SHRoles.SistemYonetici, SHRoles.PrimHakediPersoneli)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Bayi Satış Onay Veri Kaynağı", SHRoles.SistemYonetici, SHRoles.PrimHakediPersoneli)]
        public ContentResult DataSourceMix([DataSourceRequest] DataSourceRequest request)
        {

            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = VMPRD_ProductProgressPaymentModel.UpdateDataSourceFilterMix(KendoToExpression.Convert(request), userStatus);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductProgressPayment(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductProgressPaymentCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Bayi Satış Onay Veri Kaynağı", SHRoles.SistemYonetici, SHRoles.PrimHakediPersoneli)]
        public ContentResult DataSourceOperator([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = VMPRD_ProductProgressPaymentModel.UpdateDataSourceFilterOperator(KendoToExpression.Convert(request), userStatus);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductProgressPayment(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductProgressPaymentCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Bayi Satış Onay", SHRoles.SistemYonetici, SHRoles.PrimHakediPersoneli)]
        [HttpPost]
        public JsonResult Approve(VMPRD_ProductProgressPaymentModel item, string ids)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var result = new ResultStatusUI { Result = true };
            if (ids != null)
            {
                foreach (var progresPaymentId in ids.Split(','))
                {
                    var dbresult = item.Save(userStatus.user.id,Guid.Parse(progresPaymentId), Request);
                    result = new ResultStatusUI
                    {
                        Result = dbresult.result,
                        FeedBack = dbresult.result ? feedback.Success("Hakediş Tanımlama İşlemi Başarılı") : feedback.Warning(dbresult.message)
                    };
                }
            }
            else
            {
                result = new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Herhangi Bir Satış Bulunamadı."),
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
