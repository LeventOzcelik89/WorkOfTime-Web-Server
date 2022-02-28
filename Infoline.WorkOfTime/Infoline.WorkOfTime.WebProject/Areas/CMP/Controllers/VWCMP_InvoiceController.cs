using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_InvoiceController : Controller
    {
        [PageInfo("Satış Faturaları", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis)]
        public ActionResult IndexSelling()
        {
            return View();
        }

        [PageInfo("Alış Faturaları", SHRoles.OnMuhasebe, SHRoles.MuhasebeAlis)]
        public ActionResult IndexBuying()
        {
            return View();
        }

        [PageInfo("Alış Faturası Raporları", SHRoles.OnMuhasebe, SHRoles.MuhasebeAlis)]
        public ActionResult IndexBuyingReport()
        {
            return View();
        }


        [PageInfo("Faturalar Metodu", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura, SHRoles.ProjeYonetici,SHRoles.CRMBayiPersoneli,SHRoles.CagriMerkezi)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_Invoice(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_InvoiceCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Faturalar Adet Metodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var total = db.GetVWCMP_InvoiceCount(condition.Filter);
            return total;
        }

        [PageInfo("Fatura-Teklif-İrsaliye-Sipariş Dönüşümleri Metodu", SHRoles.Personel)]
        public ContentResult DataSourceInvoiceTransform([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_InvoiceTransform(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_InvoiceTransformCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Faturalar Veri Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_Invoice(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Satış Faturası Detayı", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.SatisFatura, SHRoles.ProjeYonetici)]
        public ActionResult DetailSelling(Guid id)
        {
            var data = new VMCMP_InvoiceModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Satis);
            ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
            return View(data);
        }

        [PageInfo("Alış Faturası Detayı", SHRoles.OnMuhasebe, SHRoles.ProjeYonetici, SHRoles.MuhasebeAlis)]
        public ActionResult DetailBuying(Guid id)
        {
            var data = new VMCMP_InvoiceModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Alis);
            ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
            return View(data);
        }

        [PageInfo("Alış Faturası Detayı 2", SHRoles.OnMuhasebe, SHRoles.ProjeYonetici, SHRoles.MuhasebeAlis)]
        public ActionResult DetailBuying2(Guid id)
        {
            var data = new VMCMP_InvoiceModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Alis);
            ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
            return View(data);
        }


        [PageInfo("Satış Faturası Ekleme", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.SatisFatura)]
        public ActionResult InsertSelling(VMCMP_InvoiceModels item, bool? transform = false)
        {
            item.Load(transform, (int)EnumCMP_InvoiceDirectionType.Satis);
            return View(item);
        }

        [PageInfo("Alış Faturası Ekleme", SHRoles.OnMuhasebe, SHRoles.MuhasebeAlis)]
        public ActionResult InsertBuying(VMCMP_InvoiceModels item, bool? transform = false,bool prdtransaction = false)
        {
            item.Load(transform, (int)EnumCMP_InvoiceDirectionType.Alis);
            item.type = (int)EnumCMP_InvoiceType.IrsaliyeliFatura;
            if(prdtransaction == true)
            {
                item.type = (int)EnumCMP_InvoiceType.IrsaliyesizFatura;
            }
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Fatura Ekleme", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura)]
        public JsonResult Insert(VMCMP_InvoiceModels item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Fatura kaydetme işlemi başarılı", false) :
                           feedback.Warning("Fatura kaydetme işlemi başarısız. Mesaj : " + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Fatura Not Ekleme Metodu", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura)]
        public ContentResult InsertNote(Guid invoiceId, string note)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var dbres = new VMCMP_InvoiceModels { id = invoiceId }.Load(false, null).InsertNote(userStatus.user.id, note);

            return Content(Infoline.Helper.Json.Serialize(dbres), "application/json");
        }

        [PageInfo("İşletme Aylık Gelir-Gider Verisi", SHRoles.OnMuhasebe)]
        public ContentResult GetInvoicesByCompanyMonthly(Guid companyId)
        {
            var db = new WorkOfTimeDatabase();

            var sellinginvoices = db.GetVWCMP_InvoiceSellingByCompanyId(companyId);
            var buyinginvoices = db.GetVWCMP_InvoiceBuyingByCompanyId(companyId);

            foreach (var item in sellinginvoices)
            {
                var s = item.DateMonthYear.Split('-');
                item.DateMonthYear = s[0] + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(s[1]));
            }

            foreach (var item in buyinginvoices)
            {
                var s = item.DateMonthYear.Split('-');
                item.DateMonthYear = s[0] + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(s[1]));
            }

            var data = new
            {
                Selling = sellinginvoices,
                Buying = buyinginvoices
            };

            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Projenin Faturaları Metodu", SHRoles.Personel)]
        public JsonResult CMP_InvoiceByProjectId(Guid ProjectId)
        {
            var db = new WorkOfTimeDatabase();
            var invoices = db.GetVWCMP_InvoiceByProjectId(ProjectId);

            var stock = db.GetVWCMP_InvoiceItemByInvoiceIds(invoices.Select(a => a.id).ToArray());

            var model = invoices.Select(a => new
            {
                item = a,
                stock = stock.Where(c => c.invoiceId == a.id)
            });

            return Json(model);
        }

        [PageInfo("Fatura Silme Metodu", SHRoles.OnMuhasebe, SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var prdTransaction = db.GetPRD_TransactionByInvoiceId(id).ToArray();
            var statusDone = prdTransaction.Where(a => a.status == (int)EnumPRD_TransactionStatus.islendi);

            if (statusDone.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Faturada stoklara işlenmiş bir ürün bulunduğu için fatura silinemez.")
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = new VMCMP_InvoiceModels { id = id }.Delete(prdTransaction);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Fatura Yazdır"), ExportPDF, AllowEveryone]
        public ActionResult Print(Guid id, int? type)
        {
            var model = new VMCMP_InvoiceModels { id = id }.Load(false, null).GetFormTemplate(type);
            return View(model);
        }


        [PageInfo("Döviz Düzenleme Metodu"), AllowEveryone]
        public ActionResult CurrencyUpdate(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetCMP_InvoiceById(id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Döviz Düzenleme Metodu"), AllowEveryone]
        public JsonResult CurrencyUpdate(VMCMP_OrderModels item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Döviz düzenleme işlemi başarılı", false) :
                           feedback.Warning("Döviz düzenleme işlemi başarısız. Mesaj : " + dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
