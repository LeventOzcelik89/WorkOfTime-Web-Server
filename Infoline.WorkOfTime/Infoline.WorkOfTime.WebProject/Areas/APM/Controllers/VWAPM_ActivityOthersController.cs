using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.APM.Controllers
{
	public class VWAPM_ActivityOthersController : Controller
    {
        [PageInfo("Diğer Aktiviteler Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWAPM_ActivityOthers(condition).ToDataSourceResult(request);
            data.Total = db.GetVWAPM_ActivityCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Diğer Aktiviteler Takvim Metodu", SHRoles.Personel)]
        public ContentResult DataSourceCalendar([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var activities = db.GetVWAPM_ActivityOthers(condition);
            var enumGeneric = EnumsProperties.EnumToArrayGeneric<EnumAPM_ActivityType>().ToArray();
            var enumGenericPriority = EnumsProperties.EnumToArrayGeneric<EnumVWAPM_ActivityOthers>().ToArray();

            var data = activities.Select(a => new ActivityCalendarModel
            {
                color = enumGenericPriority.Where(c => c.Key == a.type.ToString()).Select(x => x.Generic["color"]).FirstOrDefault(),
                description = a.description,
                end = a.endDate,
                start = a.startDate,
                title = a.name,
                type = a.type,
                icon = enumGeneric.Where(c => c.Key == a.type.ToString()).Select(x => x.Generic["icon"]).FirstOrDefault()
            });

            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Diğer Aktiviteler Dropdown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWAPM_ActivityOthers(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Diğer Aktiviteler Miktar DataSource", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWAPM_ActivityOthersCount(condition.Filter);
            return count;
        }
    }
}
