using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
    public class VWFTM_TaskTemplateController : Controller
    {
        [PageInfo("Görev Şablonları", SHRoles.SahaGorevYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Görev Şablonları DataSource Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 1);
            var data = db.GetVWFTM_TaskTemplate(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWFTM_TaskTemplateCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Görev Şablonları Dropdown Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWFTM_TaskTemplate(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Görev Şablonları Detay Ekranı (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ActionResult Detail(VMTaskTemplateModel template)
        {
            var model = new TaskSchedulerModel();
            model.TemplateModel = template;
            model.TemplateModel.Load();
            
            return View(model);
        }

        [PageInfo("Görev Şablonları Yeni Tanımlama Ekranı (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ActionResult Insert()
        {
            var data = new TaskSchedulerModel();
            data.TemplateModel.Load();
            return View(data);
        }

        [PageInfo("Görev Şablonları Yeni Tanımlama Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(TaskSchedulerModel item, short priority)
        {

            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];

            item.TemplateModel.priority = priority;

            var dbresult = item.TemplateModel.Insert(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev Şablonu başarıyla oluşturuldu.") :
                                             feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev Şablonu oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("Görev Şablonları Düzenleme Ekranı (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ActionResult Update(VMTaskTemplateModel template)
        {

            var model = new TaskSchedulerModel();
            model.TemplateModel = template;
            model.TemplateModel.Load();

            return View(model);
        }

        [PageInfo("Görev Şablonları Düzenleme Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(TaskSchedulerModel model, short priority)
        {

            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];

            model.TemplateModel.priority = priority;
            var dbresult = model.TemplateModel.Update(userStatus.user.id);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Error(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Görev Şablonları Silme Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {

            var feedback = new FeedBack();
            var model = new TaskSchedulerModel();
            model.TemplateModel.id = id;

            var dbresult = model.TemplateModel.Delete();

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Görev Şablonu başarıyla silindi") : feedback.Error(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
