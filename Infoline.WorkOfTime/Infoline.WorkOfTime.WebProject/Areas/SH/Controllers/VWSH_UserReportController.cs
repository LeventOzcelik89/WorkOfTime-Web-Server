using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
	public class VWSH_UserReportController : Controller
	{
		[PageInfo("Detaylı Kullanıcı Raporu Listeleme Sayfası",SHRoles.IKYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Detaylı Kullanıcı Raporu Listeleme Metodu", SHRoles.IKYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_UserReport(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_UserReportCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

	}
}
