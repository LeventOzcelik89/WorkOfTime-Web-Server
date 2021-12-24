using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
    public class VWSV_ServiceController : Controller
    {
        [AllowEveryone]
        [PageInfo("Garanti-Teknik Servis Listeleme Sayfası", SHRoles.UretimYonetici)]
        public ActionResult Index()
        {
            return View();
        }
        [AllowEveryone]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_Service(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSV_ServiceCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [AllowEveryone]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_Service(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [AllowEveryone]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_ServiceById(id);
            return View(data);
        }
        [AllowEveryone]
        public ActionResult Insert(VMSV_ServiceModel model)
        {
            model.code = BusinessExtensions.B_GetIdCode();
            return View(model);
        }
        [AllowEveryone]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMSV_ServiceModel model, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = model.Save(userStatus.user.id, HttpContext.Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Servis Kaydı Başarıyla Oluşturuldu", true, Url.Action("Index")) : feedback.Warning("Servis Kaydı Oluşturma İşlemi Başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        public ActionResult Update(VMSV_ServiceModel model)
        {
            return View(model.Load());
        }
        [AllowEveryone]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SV_Service item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var dbresult = db.UpdateSV_Service(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = id.Select(a => new SV_Service { id = new Guid(a) });
            var dbresult = db.BulkDeleteSV_Service(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        public JsonResult DeviceInformation(Guid inventoryId)
        {
            return Json(new VMSV_ServiceModel().DeviceInformation(inventoryId), JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        public ActionResult Print(Guid id)
        {
            return View(new VMSV_ServiceModel { id = id }.Load());
        }
        [AllowEveryone]
        public ContentResult ProductMaterielDataSource(Guid productId , [DataSourceRequest] DataSourceRequest request) {
            var data = new VMSV_ServiceModel().GetVWPRD_ProductMateriels(productId).GetProductMetarials.ToDataSourceResult(request); 
            return Content(Infoline.Helper.Json.Serialize(data),  "application / json");

        }
    }
}
