using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.APM.Controllers
{
	public class VWAPM_ActivityRelationController : Controller
	{
        [PageInfo("Aktivite İlişkileri Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWAPM_ActivityRelation(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWAPM_ActivityRelationCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Aktivite İlişkileri Dropdown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWAPM_ActivityRelation(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
	}
}
