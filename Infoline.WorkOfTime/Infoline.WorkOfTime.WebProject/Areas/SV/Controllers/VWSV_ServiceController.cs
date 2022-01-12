using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
using System.Linq;
namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
    public class VWSV_ServiceController : Controller
    {
        
        [PageInfo("Garanti-Teknik Servis Listeleme Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Index()
        {
            return View();
        }
        
        [PageInfo("Garanti-Teknik Servis Listeleme Methodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
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
        
        [PageInfo("Garanti-Teknik Servis DropDown Listeleme Methodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_Service(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        
        [PageInfo("Garanti-Teknik Servis Detay Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Detail(Guid id)
        {

            return View(new VMSV_ServiceModel { id = id }.Load());
        }
        
        [PageInfo("Yeni Garanti-Teknik Servis Ekleme Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Insert(VMSV_ServiceModel model)
        {
            model.code = BusinessExtensions.B_GetIdCode();
            
            return View(model);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Yeni Garanti-Teknik Servis Ekleme Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public JsonResult Insert(VMSV_ServiceModel model, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = model.Save(userStatus.user.id, HttpContext.Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Servis Kaydı Başarıyla Oluşturuldu", false, Url.Action("Index")) : feedback.Warning("Servis Kaydı Oluşturma İşlemi Başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [PageInfo(" Garanti-Teknik Servis Düzenleme Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Update(VMSV_ServiceModel model)
        {
            return View(model.Load());
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo(" Garanti-Teknik Servis Düzenleme Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public JsonResult Update(VMSV_ServiceModel model,bool? ispost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = model.Save(userStatus.user.id, HttpContext.Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı",true, Url.Action("Index")) : feedback.Warning("Güncelleme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [PageInfo(" Garanti-Teknik Servis Silme Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public JsonResult Delete(VMSV_ServiceModel model)
        {
          
            var feedback = new FeedBack();
            var dbresult = model.Delete();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Teknik Servis Kaydı Silme işlemi başarılı") : feedback.Warning(" Teknik Servis KaydıSilme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        [PageInfo(" Garanti-Teknik Servis Cihaz Bilgileri Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public JsonResult DeviceInformation(Guid inventoryId)
        {
            return Json(new VMSV_ServiceModel().DeviceInformation(inventoryId), JsonRequestBehavior.AllowGet);
        }

        [PageInfo(" Garanti-Teknik Servis Cihaz Teslim Alma Çıktı Sayfası"), AllowEveryone]
        public ActionResult Print(Guid id)
        {
            return View(new VMSV_ServiceModel { id = id }.Load());
        }      
        [PageInfo(" Garanti-Teknik Servis Cihaz Teslim Ver Çıktı Sayfası"),AllowEveryone]
        public ActionResult PrintEnd(Guid id)
        {
            return View(new VMSV_ServiceModel { id = id }.Load());
        }
        [PageInfo(" Garanti-Teknik Servis Cihaz Teslim Barkod Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Barcode(Guid id)
        {
            return View(new VMSV_ServiceModel { id = id }.Load());
        }

        [PageInfo("Garanti-Teknik Servis'e gelen cihazın metaryel ağacındaki tüm ürünleri alan metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult ProductMaterielDataSource(Guid productId , [DataSourceRequest] DataSourceRequest request) {
            var data = new VMSV_ServiceModel().GetVWPRD_ProductMateriels(productId).GetProductMetarials.ToArray(); 
            return Content(Infoline.Helper.Json.Serialize(data),  "application / json");

        }
        
        [PageInfo(" Garanti-Teknik Servis Cihazının farklı servise taşınmasını sağlayan method ", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Transfer(Guid serviceId)
        {
            return View();
        }
        
        [PageInfo(" Garanti-Teknik Servis'in Fire Verilmiş Ürünleri Alan method ", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult GetWastedProducts([DataSourceRequest] DataSourceRequest request,Guid serviceId) {

            var data = new VMSV_ServiceModel().GetWastedProducts(serviceId).ToDataSourceResult(request);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        
        [PageInfo(" Garanti-Teknik Servis'in harcama yapılmış Ürünleri Alan method ", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult GetSpendedProducts([DataSourceRequest] DataSourceRequest request,Guid serviceId)
        {
            var data = new VMSV_ServiceModel().GetSpendedProducts(serviceId).ToDataSourceResult(request);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo(" Garanti-Teknik Servis'in harcama yapılmış Ürünleri Alan method ", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ContentResult GetIndexData()
        {
            var data = new VMSV_ServiceModel().GetIndexData();
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

    }
}
