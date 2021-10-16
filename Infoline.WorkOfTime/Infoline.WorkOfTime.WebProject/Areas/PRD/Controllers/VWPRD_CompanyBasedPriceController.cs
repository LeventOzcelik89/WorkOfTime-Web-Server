using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess.Business.Product;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_CompanyBasedPriceController : Controller
    {
        [PageInfo("Ürün fiyat listeme sayfası", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Ürün fiyat listeme verileri", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPrice(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_CompanyBasedPriceCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün fiyat listesi detay sayfası", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public ActionResult Detail(Guid id)
        {
            var model = new VMPRD_CompanyBasedPriceModel { id = id };
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Ürün fiyat listesi güncelleme sayfası", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public ActionResult Update(Guid id)
        {
            var model = new VMPRD_CompanyBasedPriceModel { id = id };
            return View(model.Load());
        }
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ürün fiyat listesi güncelleme metodu", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public JsonResult Update(VMPRD_CompanyBasedPriceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [PageInfo("Ürün fiyat listesi silme metodu", SHRoles.IKYonetici, SHRoles.OnMuhasebe)]
        public JsonResult Delete(string id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbresult = new VMPRD_CompanyBasedPriceModel { id = new Guid(id) }.Delete();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Error(dbresult.message, "Silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
