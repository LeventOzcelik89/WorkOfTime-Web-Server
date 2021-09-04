using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS.Controllers
{
	public class VWPDS_FormPermitTaskUserController : Controller
    {
        [PageInfo("Değerlendirmelerim", SHRoles.Personel)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var tasks = db.GetVWPDS_FormPermitTaskUserByEvaluatorId(userStatus.user.id);
            return View(tasks);
        }

        [PageInfo("Peformans Değerlendirmesi Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormPermitTaskUser(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPDS_FormPermitTaskUserCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Peformans Değerlendirmesi Adet Metodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormPermitTaskUserCount(condition.Filter);
            return data;
        }

        [PageInfo("Peformans Değerlendirmesi Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormPermitTaskUser(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}
