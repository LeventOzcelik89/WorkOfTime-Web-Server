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

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
	public class VWINV_PermitSummaryController : Controller
	{


		[PageInfo("Çalışan İzin Özeti Verileri",SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWINV_PermitSummary(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWINV_PermitSummaryCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
		[PageInfo("Verileri")]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWINV_PermitSummary(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
	}
}
