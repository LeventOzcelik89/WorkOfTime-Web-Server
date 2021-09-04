using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
	public class VWCRM_PerformanceMultiplierController : Controller
    {
        [PageInfo("Performans Çarpanları",SHRoles.CRMYonetici)]
        public ActionResult Index()
        {

            var db = new WorkOfTimeDatabase();
            var model = db.GetVWCRM_PerformanceMultiplierGroupedPageReport().FirstOrDefault();

            return View(model);
        }


        [PageInfo("Performans Çarpanları Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_PerformanceMultiplier(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_PerformanceMultiplierCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Performans Çarpanları Veri Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_PerformanceMultiplier(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }




        [PageInfo("Performans Çarpanları Ekleme", SHRoles.CRMYonetici)]
        public ActionResult Insert(Guid? id, bool? Copy)
        {

            var db = new WorkOfTimeDatabase();

            var data = new VWCRM_PerformanceMultiplier { id = Guid.NewGuid() };
            if (id.HasValue)
            {
                var group = db.GetVWCRM_PerformanceMultiplierByGroupId(id.Value);

                if (group.Count() == 0)
                {
                    return RedirectToAction("Index");
                }

                data.GroupId = id;

                data.StartDate = group.Where(a => a.StartDate.HasValue).Select(a => a.StartDate).FirstOrDefault();
                data.EndDate = group.Where(a => a.EndDate.HasValue).Select(a => a.EndDate).FirstOrDefault();

            }

            ViewBag.Copy = Copy;

            return View(data);
        }

        [PageInfo("Performans Çarpanları Ekleme Methodu", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CRM_PerformanceMultiplier item, string PerformanceMultipliers, bool? Copy)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            var groupId = Guid.NewGuid();
            var _now = DateTime.Now;

            var datas = Infoline.Helper.Json.Deserialize<VWCRM_PerformanceMultiplier[]>(PerformanceMultipliers);
            foreach (var data in datas)
            {
                data.createdby = userStatus.user.id;
                data.created = _now;
                data.GroupId = groupId;
                data.StartDate = item.StartDate;
                data.EndDate = item.EndDate;
            }

            var trans = db.BeginTransaction();
            var dbRes = new ResultStatus { result = true };

            if (Copy == true)
            {
                dbRes = db.BulkInsertCRM_PerformanceMultiplier(datas.ConvertType<CRM_PerformanceMultiplier>(), trans);
            }
            else
            {

                //  Düzenle
                if (item.GroupId.HasValue)
                {

                    var currentPerformances = db.GetVWCRM_PerformanceMultiplierByGroupId(item.GroupId.Value);
                    dbRes = db.BulkDeleteCRM_PerformanceMultiplier(currentPerformances.ConvertType<CRM_PerformanceMultiplier>(), trans);

                }

                dbRes = db.BulkInsertCRM_PerformanceMultiplier(datas.ConvertType<CRM_PerformanceMultiplier>(), trans);

            }

            if (dbRes.result)
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Performans Çarpan Değerlerini Tanımlama işlemi başarıyla tamamlandı...", false, Url.Action("Index", "VWCRM_PerformanceMultiplier", new { area = "CRM" }))
                }, JsonRequestBehavior.AllowGet);

            }

            trans.Rollback();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Error(dbRes.message)
            }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        [PageInfo("Performans Çarpanları Silme",SHRoles.CRMYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var datas = db.GetVWCRM_PerformanceMultiplierByGroupId(id);
            var dbresult = db.BulkDeleteCRM_PerformanceMultiplier(datas.ConvertType<CRM_PerformanceMultiplier>());

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Performans Çarpan Değerleri başarıyla silindi...") : feedback.Error(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Çarpan Grubu Ekleme Methodu", SHRoles.CRMYonetici)]
        public ContentResult GetMultipliersByGroupId(Guid? groupId)
        {

            var db = new WorkOfTimeDatabase();

            if (groupId.HasValue)
            {
                var data = db.GetVWCRM_PerformanceMultiplierByGroupId(groupId.Value);
                return Content(Infoline.Helper.Json.Serialize(data), "application/json");
            }

            return Content(Infoline.Helper.Json.Serialize(new VWCRM_MonthlyTarget[0]), "application/json");
        }
    }
}
