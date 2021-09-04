using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_StockActionController : Controller
	{
		[PageInfo("Stok Hareketleri", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Ürün Stok Hareketleri Veri Metodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_StockAction(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_StockActionCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Stok Hareketleri Veri Metodu", SHRoles.Personel)]
		public ContentResult DataSourceIndex([DataSourceRequest] DataSourceRequest request)
		{
			var db = new WorkOfTimeDatabase();
			var condition = KendoToExpression.Convert(request);
			var mycompanies = db.GetCMP_CompanyByType(EnumCMP_CompanyType.Benimisletmem).ToList();
			condition.Filter &= new BEXP
			{
				Operand1 = (COL)"stockCompanyId",
				Operator = BinaryOperator.In,
				Operand2 = new ARR { Values = mycompanies.Select(a => (VAL)a).ToArray() },
			};
			request.Page = 1;
			var data = db.GetVWPRD_StockAction(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWPRD_StockActionCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Stok Hareketleri Veri Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_StockAction(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Stok Hareketleri Detayı", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_StockActionById(id);
			return View(data);
		}
	}
}
