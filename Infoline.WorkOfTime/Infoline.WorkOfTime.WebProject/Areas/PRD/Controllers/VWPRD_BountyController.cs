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
    public class VWPRD_BountyController : Controller
    {
        [PageInfo("Hakediş Raporu", SHRoles.SistemYonetici, SHRoles.PrimHakedisPersoneli)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Hakediş Raporu Veri Kaynağı", SHRoles.SistemYonetici, SHRoles.PrimHakedisPersoneli)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Bounty(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_BountyCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Hakedişi Olan Bayi Dropdown Verileri", SHRoles.PrimHakedisPersoneli)]
        public ContentResult DataSourceDropDownPaymentBayi([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var payment = db.GetVWPRD_Bounty();
            if (payment != null)
            {
                var filterCompany = string.Join(",", payment.GroupBy(a => a.companyId).Select(c => c.Key).ToArray());
                if (filterCompany != null)
                {
                    var filtersSplit = filterCompany.Split(',');
                    if (filtersSplit != null)
                    {
                        foreach (var item in filtersSplit)
                        {
                            condition.Filter |= new BEXP
                            {
                                Operand1 = (COL)"id",
                                Operand2 = (VAL)item,
                                Operator = BinaryOperator.Equal
                            };
                        }
                    }
                }
            }
            var data = db.GetVWCMP_Company(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Hakediş Raporu Veri kaynağı", SHRoles.SistemYonetici, SHRoles.PrimHakedisPersoneli)]
        public ContentResult ReportDataSource(Guid companyId, Guid bonusId, int status)
        {
            var model = new VMPRD_BountyModel();
            status = status == 1 ? (int)EnumPRD_BountyStatus.completePayment : (int)EnumPRD_BountyStatus.completePayment;
            var data = model.ReportData(companyId, bonusId, status);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Hakediş Ödeme", SHRoles.SistemYonetici, SHRoles.PrimHakedisPersoneli)]
        public JsonResult Approve(VMPRD_BountyModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var data = item.Load();
            var dbresult = item.Approve(data, userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Hakediş Ödeme İşlemi Başarılı", false, null, 1) : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
