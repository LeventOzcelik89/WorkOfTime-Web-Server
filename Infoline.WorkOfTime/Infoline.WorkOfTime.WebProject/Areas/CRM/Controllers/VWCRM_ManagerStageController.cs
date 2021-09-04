using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
	public class VWCRM_ManagerStageController : Controller
    {
        [PageInfo("Potansiyel/Fırsat Aşamaları", SHRoles.CRMYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Potansiyel/Fırsat Aşaması Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ManagerStage(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_ManagerStageCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Potansiyel/Fırsat Aşaması Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ManagerStage(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Potansiyel/Fırsat Aşaması Detayı", SHRoles.CRMYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ManagerStageById(id);
            return View(data);
        }


        [PageInfo("Potansiyel/Fırsat Aşaması Ekleme", SHRoles.CRMYonetici)]
        public ActionResult Insert()
        {
            var data = new VWCRM_ManagerStage { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("Potansiyel/Fırsat Aşaması Ekleme Methodu", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CRM_ManagerStage item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var control = db.GetCRM_ManagerStageControl(item.Code);

            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı kodlu kayıt bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertCRM_ManagerStage(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Aşama ekleme işlemi başarı ile gerçekleşti.") : feedback.Error("Aşama ekleme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Potansiyel/Fırsat Aşaması Güncelleme", SHRoles.CRMYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ManagerStageById(id);
            return View(data);
        }


        [PageInfo("Potansiyel/Fırsat Aşaması Güncelleme Methodu", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CRM_ManagerStage item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();




            var control = db.GetCRM_ManagerStageUpdate(item.id, item.Code);

            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kayıt Zaten Bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateCRM_ManagerStage(item, true);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Aşama güncelleme işlemi başarı ile gerçekleşti.") : feedback.Error("Aşama güncelleme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Potansiyel/Fırsat Aşaması Silme", SHRoles.CRMYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new CRM_ManagerStage { id = new Guid(a) });
            var data = db.GetVWCRM_PresentationByPresentationStageId(id).Count();
            //var presentationItem = id.Select(a => new CRM_Presentation { }).Count();
            //var presentationItem = id.Select(a => new CRM_Presentation { }).Where(a => a.PresentationStageId.ToString() == id.ToString()).Count();
            //var presentationItem = id.Select(a => new CRM_Presentation { id = new Guid(a) }).Count();
            if (data > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bu aşamaya ait potansiyel kaydı olduğundan silemezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }


            var dbresult = db.BulkDeleteCRM_ManagerStage(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Aşama silme işlemi başarı ile gerçekleşti.") : feedback.Error("Aşama silme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
