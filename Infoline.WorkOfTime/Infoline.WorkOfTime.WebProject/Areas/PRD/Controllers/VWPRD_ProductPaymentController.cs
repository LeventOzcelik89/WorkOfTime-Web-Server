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
	public class VWPRD_ProductPaymentController : Controller
	{
		[PageInfo("Hakediş Raporu", SHRoles.SistemYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Hakediş Raporu Veri Kaynağı", SHRoles.SistemYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPayment(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_ProductPaymentCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Hakediş Raporu Detay", SHRoles.SistemYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPaymentById(id);
		    return View(data);
		}
        [PageInfo("Hakediş Raporu Veri kaynağı", SHRoles.SistemYonetici)]
        public ContentResult ReportDataSource(Guid companyId, int year, int month)
        {
            var model = new VMPRD_ProductPaymentModel();
            var data = model.ReportData(companyId,year,month);
            return Content(Infoline.Helper.Json.Serialize(data),"application/json");
        }


		[PageInfo("Hakediş Ödeme", SHRoles.SistemYonetici)]
		public JsonResult Approve(VMPRD_ProductPaymentModel item)
		{
			var userStatus = (PageSecurity)Session["userStatus"]; 
			var feedback = new FeedBack();
			var data = item.Load();
			var dbresult = item.Approve(data,userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Hakediş Ödeme İşlemi Başarılı",false,null,1) : feedback.Warning(dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
