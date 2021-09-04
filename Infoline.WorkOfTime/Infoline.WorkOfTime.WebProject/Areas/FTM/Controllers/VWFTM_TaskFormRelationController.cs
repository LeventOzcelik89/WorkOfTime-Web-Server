using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
	public class VWFTM_TaskFormRelationController : Controller
	{
        [PageInfo("Envantar Görev Formları Bağlantı Verileri Methodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskFormRelation(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWFTM_TaskFormRelationCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Envantar Görev Formları Bağlantı Dropdown Methodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskFormRelation(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
	}
}
