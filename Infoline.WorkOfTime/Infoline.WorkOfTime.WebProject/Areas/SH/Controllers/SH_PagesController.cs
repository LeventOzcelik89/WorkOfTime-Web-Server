using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BotDetect.C5;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class SH_PagesController : Controller
    {
        [PageInfo("Sayfa Havuzu Methodu",SHRoles.SistemYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_Pages(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetSH_PagesCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Sayfa Havuzu Veri Methodu", SHRoles.SistemYonetici)]
        public JsonResult DataSourceDrop([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_Pages(condition).Select(x => new { Id = x.id, Description = !string.IsNullOrEmpty(x.Description) ? x.Description : x.Action });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}

