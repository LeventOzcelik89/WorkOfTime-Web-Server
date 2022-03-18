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
		[PageInfo("Hakediş Onay Listesi", SHRoles.SistemYonetici)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Hakediş Onay Veri Kaynağı", SHRoles.SistemYonetici)]
		public ContentResult DataSourceMix([DataSourceRequest]DataSourceRequest request)
		{
		    
			var userStatus = (PageSecurity)Session["userStatus"];
			var condition = VMPRD_ProductProgressPaymentModel.UpdateDataSourceFilterMix(KendoToExpression.Convert(request), userStatus);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductProgressPayment(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWPRD_ProductProgressPaymentCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Hakediş Onay Veri Kaynağı", SHRoles.SistemYonetici)]
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


		[PageInfo("Hakediş Onay", SHRoles.SistemYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Approve(VMPRD_ProductProgressPaymentModel item)
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
	}
}
