using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess.Business.Product;
using Infoline.WorkOfTime.BusinessAccess.Models;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    [AllowEveryone]
    public class PRD_TitanDeviceActivatedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
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
        public JsonResult Insert()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = new VMPRD_TitanDeviceActivated().SaveAll(userStatus.user.id);
            return Json(data,JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetAllDevices()
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetAllDevices(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeviceActivationInformation()
        {
            VMPRD_TitanDeviceActivated model = new VMPRD_TitanDeviceActivated();
            return Json(model.GetDeviceActivationInformation(), JsonRequestBehavior.AllowGet);
        }
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
