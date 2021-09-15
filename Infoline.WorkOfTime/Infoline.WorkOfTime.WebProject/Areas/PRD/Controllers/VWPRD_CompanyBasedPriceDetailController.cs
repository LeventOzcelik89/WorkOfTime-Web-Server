using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess.Business.Product;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_CompanyBasedPriceDetailController : Controller
    {

        [PageInfo("Verileri listeleyen metod",SHRoles.IKYonetici,SHRoles.OnMuhasebe)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPriceDetail(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_CompanyBasedPriceDetailCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Ürün fiyat liste ekleme sayfası", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public ActionResult Insert()
        {
            var data = new VMPRD_CompanyBasedPriceDetailModel { id = Guid.NewGuid(), companyBasedPriceId = Guid.NewGuid() };
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ürün fiyat liste ekleme metodu", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public JsonResult Insert(VMPRD_CompanyBasedPriceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.createdby = userStatus.user.id;
            item.created = DateTime.Now;
            var dbresult = item.Save();
            var feedback = new FeedBack();
            if (dbresult.result != false)
            {
                var result = new ResultStatusUI()
                {
                    Result = dbresult.result,
                    FeedBack = feedback.Success("Özel Fiyat Belirleme İşlemi Tamamlandı", false, Url.Action("Index", "VWPRD_CompanyBasedPrice", new { area = "PRD" }))
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Özel Fiyat Belirleme İşlemi Başarılı") : feedback.Warning(dbresult.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
      
        [PageInfo("Ürün fiyat liste güncelleme sayfası", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public ActionResult Update(Guid id)
        {
            var data = new VMPRD_CompanyBasedPriceDetailModel { id = id };
            return View(data.Load());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        
        [PageInfo("Ürün fiyat liste güncelleme metodu", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public JsonResult Update(VMPRD_CompanyBasedPriceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.createdby = userStatus.user.id;
            item.created = DateTime.Now;
            var dbresult = item.Save();
            var feedback = new FeedBack();
            if (dbresult.result != false)
            {
                var result = new ResultStatusUI()
                {
                    Result = dbresult.result,
                    FeedBack = feedback.Success("Özel Fiyat Belirleme Güncelleme  İşlemi Tamamlandı", false, Url.Action("Index", "VWPRD_CompanyBasedPrice", new { area = "PRD" }))
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Özel Fiyat Belirleme Güncelleme İşlemi Başarılı") : feedback.Warning(dbresult.message)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [PageInfo("Ürün fiyat liste silme metodu", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public JsonResult Delete(VMPRD_CompanyBasedPriceDetailModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var dbresult = item.Delete();
            var feedback = new FeedBack();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Ürün Fiyatı Silme İşlemi Başarılı") : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
