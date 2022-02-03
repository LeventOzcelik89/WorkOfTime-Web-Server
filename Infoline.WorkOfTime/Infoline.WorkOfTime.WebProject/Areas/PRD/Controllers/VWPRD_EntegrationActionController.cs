using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Business.Product;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_EntegrationActionController : Controller
    {
        [AllowEveryone]
        [PageInfo("Bayi Aktivasyon Raporu Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator  )]
        public ActionResult SellerReport(PRD_EntegrastionActionSellerReport model)
        {
            return View(model.Load());
        }


        [AllowEveryone]
        [PageInfo("Bayi Aktivasyon Raporu DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_EntegrationAction(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_EntegrationActionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}
