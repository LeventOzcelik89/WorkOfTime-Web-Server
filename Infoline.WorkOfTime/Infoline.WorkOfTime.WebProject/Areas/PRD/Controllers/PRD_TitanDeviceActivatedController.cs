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
            var data = db.GetPRD_TitanDeviceActivated(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetPRD_TitanDeviceActivatedCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetPRD_TitanDeviceActivated(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        public ActionResult Detail(VMPRD_TitanDeviceActivated model)
        {

            var data = model.Load();
            return View(data);
        }
        public ActionResult Insert()
        {
            var data = new PRD_TitanDeviceActivated { id = Guid.NewGuid() };
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMPRD_TitanDeviceActivated item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = item.Save();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public ActionResult Update(VMPRD_TitanDeviceActivated model)
        {
            var data = model.Load();
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMPRD_TitanDeviceActivated item,bool isPost)
        {
         
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var dbresult = item.Save();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(VMPRD_TitanDeviceActivated item)
        {    
            var feedback = new FeedBack();
            var dbresult = item.Delete();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _Detail(VMPRD_TitanDeviceActivated model)
        {
            var data = model.Load();
            return View(data);
        }

    }
}
