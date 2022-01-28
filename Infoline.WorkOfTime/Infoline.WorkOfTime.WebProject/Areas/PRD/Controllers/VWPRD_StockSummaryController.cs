using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_StockSummaryController : Controller
    {
        [PageInfo("Stok Özetleri", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaOnaylayici, SHRoles.SatisOnaylayici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Ürün Stok Özeti", SHRoles.Personel)]
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
            var data = db.GetVWPRD_StockSummary(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_StockSummaryCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Stok Özeti", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StockSummary(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_StockSummaryCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Stok Özeti", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StockSummary(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürünlerin Stok Özeti", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatisPersoneli, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaOnaylayici, SHRoles.SatisOnaylayici,SHRoles.CRMBayiPersoneli,SHRoles.CagriMerkezi)]
        public ActionResult StockDetailProducts(string productIds)
        {
            var model = productIds.Split(',').Select(a => (Guid?)new Guid(a)).ToList();
            return View(new TempModel
            {
                ProductIds = model,
            });
        }
        [PageInfo("Ürün Stok Özetlerinin Hareket Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid? productId, Guid? stockId, string stockTable)
        {
            var model = new SummaryActionModel()
            {
                ProductId = productId,
                StockId = stockId,
                StockTable = stockTable
            };
            return View(model);
        }
    }
}
