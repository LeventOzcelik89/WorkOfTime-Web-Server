using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_InvoiceItemController : Controller
    {
        [PageInfo("Ürün Kalemleri (Talep, Fatura, Teklif, İrsaliye, Sipariş)", SHRoles.Personel,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_InvoiceItem(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_InvoiceItemCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Bazlı Rapor (Talep, Fatura, Teklif, İrsaliye, Sipariş)", SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi)]
        public ContentResult DataSourceReport([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_InvoiceItemReport(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_InvoiceItemReportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Kalemleri Dropdown (Talep, Fatura, Teklif, İrsaliye, Sipariş)", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_InvoiceItem(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Faturalardaki Ürün Kalemleri", SHRoles.Personel)]
        public ContentResult GetInvoiceItemsByInvoiceIds(string[] invoiceIds)
        {
            var db = new WorkOfTimeDatabase();
            var items = db.GetVWCMP_InvoiceItemByInvoiceIds(invoiceIds.Select(a => Guid.Parse(a)).ToArray());

            return Content(Infoline.Helper.Json.Serialize(items), "application/json");
        }
    }
}
