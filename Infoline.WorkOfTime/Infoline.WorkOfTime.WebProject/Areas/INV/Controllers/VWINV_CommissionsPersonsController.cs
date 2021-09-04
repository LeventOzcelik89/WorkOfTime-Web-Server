using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
	public class VWINV_CommissionsPersonsController : Controller
    {
        [PageInfo("Personel Görevlendirmeleri Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CommissionsPersons(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CommissionsPersonsCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel Görevlendirmeleri Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWINV_CommissionsPersonsCount(condition.Filter);
            return count;
        }
    }
}
