using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
	public class VWSYS_NotificationController : Controller
	{
		[PageInfo("Bildirimlerin Listelendiği Sayfa",SHRoles.SistemYonetici)]
		public ActionResult Index()
		{
		    return View();
		}
		[PageInfo("Bildirimlerin Listelendiği Metod", SHRoles.SistemYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSYS_Notification(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSYS_NotificationCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Bildirimlerin Listelendiği Metod", SHRoles.SistemYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSYS_Notification(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Bildirimlerin Listelendiği Sayfa", SHRoles.SistemYonetici)]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSYS_NotificationModel { id = id }.Load());
		}
		[PageInfo("Bildirimlerin Listelendiği Sayfa", SHRoles.SistemYonetici)]
		public ActionResult Insert(Guid id)
		{
			return View(new VMSYS_NotificationModel { id = id }.Load());
		}
		public JsonResult Insert(VMSYS_NotificationModel model) {

			var feedback = new FeedBack();
			new BusinessAccess.Notification().NotificationSend(model.userId.Value, model.title, model.message,model.url,model.paramaters,"",null,false);
			var result = new ResultStatusUI
			{

				Result = true,
				FeedBack = feedback.Success("Bildirim Gönderildi")
			};
			return Json(result, JsonRequestBehavior.AllowGet);

		}
	}
}
