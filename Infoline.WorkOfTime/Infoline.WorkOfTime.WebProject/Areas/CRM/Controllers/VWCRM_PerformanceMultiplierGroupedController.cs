using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
    public class VWCRM_PerformanceMultiplierGroupedController : Controller
	{
        [PageInfo("Performans Çarpanı Grup Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCRM_PerformanceMultiplierGrouped(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWCRM_PerformanceMultiplierGroupedCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Performans Çarpanı Grup Veri Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCRM_PerformanceMultiplierGrouped(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
	}
}
