using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductPriceController : Controller
    {
        [PageInfo("Ürün Fiyat Geçmişi", SHRoles.StokYoneticisi, SHRoles.SatisOnaylayici, SHRoles.DepoSorumlusu)]
        public ActionResult Index(PRD_ProductPrice item)
        {
            return View(item);
        }

        [PageInfo("Ürün Fiyat Geçmişi", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductPrice(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductPriceCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}
