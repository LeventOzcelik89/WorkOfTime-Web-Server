using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_InventoryActionController : Controller
    {
        [PageInfo("Envanter Hareketleri", SHRoles.Personel, SHRoles.SahaGorevMusteri)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_InventoryAction(condition).ToDataSourceResult(request);
            data.Total = db.GetVWPRD_InventoryActionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}
