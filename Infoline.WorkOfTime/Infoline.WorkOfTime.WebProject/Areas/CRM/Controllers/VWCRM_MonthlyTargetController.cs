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
	public class VWCRM_MonthlyTargetController : Controller
    {
        [PageInfo("Aylık Hedefler",SHRoles.CRMYonetici)]
        public ActionResult Index()
        {

            var db = new WorkOfTimeDatabase();
            var model = db.GetVWCRM_MonthlyTargetGroupedPageReport().FirstOrDefault();

            return View(model);
        }


        [PageInfo("Aylık Hedefler Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_MonthlyTarget(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_MonthlyTargetCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Aylık Hedefler Veri Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_MonthlyTarget(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }



        [PageInfo("Aylık Hedef Ekleme", SHRoles.CRMYonetici)]
        public ActionResult Insert(Guid? id, bool? Copy)
        {

            var db = new WorkOfTimeDatabase();

            var data = new VWCRM_MonthlyTarget { id = Guid.NewGuid() };
            if (id.HasValue)
            {
                var group = db.GetVWCRM_MonthlyTargetByGroupId(id.Value);

                if (group.Count() == 0)
                {
                    return RedirectToAction("Index");
                }

                data.GroupId = id;

                data.CPeriod = group.Where(a => a.CPeriod.HasValue).Select(a => a.CPeriod).FirstOrDefault();

            }

            ViewBag.Copy = Copy;

            return View(data);

        }

        [PageInfo("Aylık Hedef Ekleme Methodu", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CRM_MonthlyTarget item, string MonthlyTargets, bool? Copy)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            var groupId = Guid.NewGuid();
            var _now = DateTime.Now;

            var datas = Infoline.Helper.Json.Deserialize<CRM_MonthlyTarget[]>(MonthlyTargets);
            foreach (var data in datas)
            {
                data.createdby = userStatus.user.id;
                data.created = _now;
                data.GroupId = groupId;
                data.CPeriod = item.CPeriod;
            }

            var trans = db.BeginTransaction();
            var dbRes = new ResultStatus { result = true };

            if (Copy == true)
            {
                dbRes &= db.BulkInsertCRM_MonthlyTarget(datas, trans);
            }
            else
            {

                //  Düzenle
                if (item.GroupId.HasValue)
                {

                    var currentTargets = db.GetVWCRM_MonthlyTargetByGroupId(item.GroupId.Value);
                    dbRes &= db.BulkDeleteCRM_MonthlyTarget(currentTargets.ConvertType<CRM_MonthlyTarget>(), trans);

                }

                dbRes &= db.BulkInsertCRM_MonthlyTarget(datas, trans);

            }

            if (dbRes.result)
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Aylık Hedef Değerleri Tanımlama işlemi başarıyla tamamlandı...", false, Url.Action("Index", "VWCRM_MonthlyTarget", new { area = "CRM" }))
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
        [PageInfo("Aylık Hedef Silme", SHRoles.CRMYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var datas = db.GetVWCRM_MonthlyTargetByGroupId(id);
            var dbresult = db.BulkDeleteCRM_MonthlyTarget(datas.ConvertType<CRM_MonthlyTarget>());

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Aylık Hedef Değerleri başarıyla silindi...") : feedback.Error(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Hedefe Group Ekleme Methodu", SHRoles.CRMYonetici)]
        public ContentResult GetTargetsByGroupId(Guid? groupId)
        {

            var db = new WorkOfTimeDatabase();

            if (groupId.HasValue)
            {
                var data = db.GetVWCRM_MonthlyTargetByGroupId(groupId.Value);
                return Content(Infoline.Helper.Json.Serialize(data), "application/json");
            }

            return Content(Infoline.Helper.Json.Serialize(new VWCRM_MonthlyTarget[0]), "application/json");

        }
    }
}
