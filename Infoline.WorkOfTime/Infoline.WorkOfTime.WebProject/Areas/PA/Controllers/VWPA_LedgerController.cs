using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PA.Controllers
{
    public class VWPA_LedgerController : Controller
    {
        [PageInfo("Kasa&Banka Raporu", SHRoles.OnMuhasebe)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var myAccountIds = db.GetVWPA_AccountMy();
            var ids = myAccountIds.Select(x => x.id).ToArray();
            var data = db.GetVWPA_LedgerbyAccountIds(ids);
            var model = new VMPA_Ledger();
            model.Ledgers = data;
            model.AccountIds = myAccountIds;
            return View(model);
        }

        [PageInfo("Gerçekleşen Ödeme İşlemleri Methodu", SHRoles.OnMuhasebe)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Ledger(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPA_LedgerCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Gerçekleşen Ödeme İşlemleri Veri Methodu", SHRoles.OnMuhasebe)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Ledger(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Gerçekleşen Ödeme İşlemleri Detay Sayfası", SHRoles.OnMuhasebe)]
        public ActionResult Detail(Guid? id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_LedgerById((Guid)id);
            return View(data);
        }

        [PageInfo("Gerçekleşen Ödeme İşlemleri Ekleme Sayfası", SHRoles.OnMuhasebe, SHRoles.SatisFatura, SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis)]
        public ActionResult Insert(VMPA_LedgerModel model)
        {
            model.Load();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Gerçekleşen Ödeme İşlemleri Ekleme Sayfası", SHRoles.OnMuhasebe, SHRoles.SatisFatura, SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis)]
        public JsonResult Insert(VMPA_LedgerModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Gerçekleşen Ödeme İşlemleri Güncelleme Sayfası", SHRoles.OnMuhasebe)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = new VMPA_LedgerModel { id = id }.Load();
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Gerçekleşen Ödeme İşlemleri Güncelleme Sayfası", SHRoles.OnMuhasebe)]
        public JsonResult Update(VMPA_LedgerModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = item.Save(userStatus.user.id, Request);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Gerçekleşen Ödeme İşlemleri Silme İşlemi", SHRoles.OnMuhasebe)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new PA_Ledger { id = new Guid(a) });

            var dbresult = db.BulkDeletePA_Ledger(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

