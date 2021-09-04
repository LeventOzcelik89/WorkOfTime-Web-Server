using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
	public class VWCRM_MonthlyTargetPersonGroupedController : Controller
	{
        [PageInfo("Personel Aylık Grup Hedefleri Sayfası", SHRoles.CRMYonetici)]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Personel Aylık Grup Hedefleri Veri Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCRM_MonthlyTargetPersonGrouped(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWCRM_MonthlyTargetPersonGroupedCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Personel Aylık Grup Hedefleri Dropdown Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCRM_MonthlyTargetPersonGrouped(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Personel Aylık Grup Hedefleri Detayı", SHRoles.CRMYonetici)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCRM_MonthlyTargetPersonGroupedById(id);
		    return View(data);
		}

        [PageInfo("Personel Aylık Grup Hedefleri Ekleme", SHRoles.CRMYonetici)]
        public ActionResult Insert()
		{
		    var data = new VWCRM_MonthlyTargetPersonGrouped { id = Guid.NewGuid() };
		    return View(data);
		}
	}
}
