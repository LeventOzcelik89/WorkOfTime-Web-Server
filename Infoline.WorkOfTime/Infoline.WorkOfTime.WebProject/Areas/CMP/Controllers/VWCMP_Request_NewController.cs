using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
    public class VWCMP_Request_NewController : Controller
	{
		
		[AllowEveryone]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCMP_Request_New(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWCMP_Request_NewCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}




	}
}
