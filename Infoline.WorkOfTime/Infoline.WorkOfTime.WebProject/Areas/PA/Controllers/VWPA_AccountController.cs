using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PA.Controllers
{
    public class VWPA_AccountController : Controller
    {
        [PageInfo("Kasa ve Bankalar", SHRoles.OnMuhasebe)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Methodu", SHRoles.OnMuhasebe, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Account(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPA_AccountCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Methodu", SHRoles.OnMuhasebe)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var total = db.GetVWPA_AccountCount(condition.Filter);
            return total;
        }

        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Veri Methodu", SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Account(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Detay Sayfası", SHRoles.OnMuhasebe)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = new VMPA_AccountModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Ekleme Sayfası", SHRoles.OnMuhasebe, SHRoles.IKYonetici,SHRoles.CRMBayiPersoneli)]
        public ActionResult Insert(VMPA_AccountModel item)
        {
            item.Load();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Ekleme Sayfası", SHRoles.OnMuhasebe, SHRoles.IKYonetici, SHRoles.CRMBayiPersoneli)]
        public JsonResult Insert(VMPA_AccountModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Güncelleme Sayfası", SHRoles.OnMuhasebe, SHRoles.IKYonetici)]
        public ActionResult Update(VMPA_AccountModel item)
        {
            item.Load();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Güncelleme Sayfası", SHRoles.OnMuhasebe, SHRoles.IKYonetici)]
        public JsonResult Update(VMPA_AccountModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Sime İşlemi", SHRoles.OnMuhasebe)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var dbRes = new VMPA_AccountModel { id = id }.Load().Delete();

            return Json(new ResultStatusUI
            {
                Result = dbRes.result,
                FeedBack = dbRes.result ? feedback.Success(dbRes.message) : feedback.Warning(dbRes.message)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
