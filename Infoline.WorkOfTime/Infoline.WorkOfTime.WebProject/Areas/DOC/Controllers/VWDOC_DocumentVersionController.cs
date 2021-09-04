using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.DOC.Controllers
{
    public class VWDOC_DocumentVersionController : Controller
    {
      
        [PageInfo("Doküman Versiyon Yönetimi Veri Methodu", SHRoles.DokumanYonetimRolu)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWDOC_DocumentVersion(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWDOC_DocumentVersionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Doküman Versiyon Detayı", SHRoles.DokumanYonetimRolu)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var version = db.GetDOC_DocumentVersionById(id);
            return View(version);
        }

    }
}
