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
	public class VWPRD_ProductionProductController : Controller
	{
		[PageInfo("Üretim Emri Ürünleri Grid Metodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductionProduct(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_ProductionProductCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Üretim Emri Ürünleri Dropdown Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductionProduct(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
	}
}
