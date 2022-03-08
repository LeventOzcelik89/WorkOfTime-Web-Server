using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class SYS_EnumsController : Controller
    {
		[AllowEveryone]
		[PageInfo("Enum Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetSYS_Enums(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
	}
}
