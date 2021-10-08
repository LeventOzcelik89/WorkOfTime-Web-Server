using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess.Business.Product;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    [AllowEveryone]
    public class PRD_TitanDeviceActivatedController : Controller
    {
        [PageInfo("Titan Cihaz Listeleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public ActionResult Index()
        {
            ViewBag.data= new VMPRD_TitanDeviceActivated().GetIndexData();
            return View();
        }
       
      
        [PageInfo("Titan Cihaz Listeleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var dataValues = db.GetVWPRD_TitanDeviceActivated(condition);
            dataValues.Each(x => x.id = (Guid)(x.InventoryId.HasValue==true?x.InventoryId:new Guid()));
            var data = dataValues.RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_TitanDeviceActivatedCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Titan Tüm Cihazları Alma Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetAllDevices()
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetAllDevices(), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihazları Detaylı Bilgi Alma Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetDeviceActivationInformation()
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceActivationInformation(), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Titan Cihazları Detaylı Bilgi Alma Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
        public JsonResult GetDeviceById(Guid id)
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceById(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeviceInformation(Guid id)
        {

            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceInformation(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _Detail(VMPRD_TitanDeviceActivated model)
        {
            var data = model.Load();
            return View(data);
        }

    }
}
