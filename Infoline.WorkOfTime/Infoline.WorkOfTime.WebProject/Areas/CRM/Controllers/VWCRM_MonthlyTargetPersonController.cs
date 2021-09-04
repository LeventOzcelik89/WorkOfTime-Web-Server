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
	public class VWCRM_MonthlyTargetPersonController : Controller
    {
        [PageInfo("Personel Aylık Hedefleri", SHRoles.CRMYonetici)]
        public ActionResult Index()
        {

            var db = new WorkOfTimeDatabase();
            var model = db.GetVWCRM_MonthlyTargetGroupedPersonPageReport().FirstOrDefault();

            return View(model);
        }

        [PageInfo("Personel Aylık Hedefleri Veri Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_MonthlyTargetPerson(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_MonthlyTargetPersonCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Personel Aylık Hedefleri Dropdown Veri Methodu", SHRoles.CRMYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_MonthlyTargetPerson(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Personel Aylık Hedefleri Ekleme", SHRoles.CRMYonetici)]
        public ActionResult Insert(Guid? id, bool? Copy)
        {

            var db = new WorkOfTimeDatabase();

            var data = new VWCRM_MonthlyTargetPerson { id = Guid.NewGuid() };
            if (id.HasValue)
            {
                var group = db.GetVWCRM_MonthlyTargetPersonByGroupId(id.Value);

                if (group.Count() == 0)
                {
                    return RedirectToAction("Index");
                }

                data.GroupId = id;

                data.CPeriod = group.Where(a => a.CPeriod.HasValue).Select(a => a.CPeriod).FirstOrDefault();

                data.TargetUserId = group.Where(a => a.TargetUserId.HasValue).Select(a => a.TargetUserId).FirstOrDefault();

            }

            ViewBag.Copy = Copy;

            return View(data);

        }


        [PageInfo("Personel Aylık Hedefleri Ekleme Methodu", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CRM_MonthlyTargetPerson item, string MonthlyTargets, bool? Copy, Guid[] TargetUserIds)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            var _now = DateTime.Now;

            if (!item.CPeriod.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Hedef Tarihi seçimi gerçekleştirmediniz. İşleme devam edilemiyor !...")
                });
            }

            if (TargetUserIds.Count() <= 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Hiçbir Personel seçimi gerçekleştirmediniz. İşleme devam edilemiyor !...")
                });
            }

            var datas = Infoline.Helper.Json.Deserialize<CRM_MonthlyTargetPerson[]>(MonthlyTargets);
            var insertList = new List<CRM_MonthlyTargetPerson>();

            foreach (var targetUserId in TargetUserIds)
            {

                var groupId = Guid.NewGuid();
                var dataList = new List<CRM_MonthlyTargetPerson>();

                foreach (var data in datas)
                {
                    dataList.Add(new CRM_MonthlyTargetPerson
                    {
                        createdby = userStatus.user.id,
                        created = _now,
                        GroupId = groupId,
                        CPeriod = item.CPeriod,
                        TargetUserId = targetUserId,
                        BonusPercentage = data.BonusPercentage,
                        IsFocus = data.IsFocus,
                        ProductGroupId = data.ProductGroupId,
                        TargetPercent = data.TargetPercent,
                        TargetPoint = data.TargetPoint,
                        IsMultipleFocus = data.IsMultipleFocus,
                        RowId = data.RowId
                    });

                }

                insertList.AddRange(dataList);

            }

            var trans = db.BeginTransaction();
            var dbRes = new ResultStatus { result = true };
            var currentDatas = db.GetVWCRM_MonthlyTargetPersonByCPeriodPersonIds(item.CPeriod.Value, TargetUserIds);

            dbRes &= db.BulkDeleteCRM_MonthlyTargetPerson(currentDatas.ConvertType<CRM_MonthlyTargetPerson>(), trans);

            if (Copy == true)
            {
                dbRes &= db.BulkInsertCRM_MonthlyTargetPerson(insertList, trans);
            }
            else
            {

                //  Düzenle
                if (item.GroupId.HasValue)
                {

                    var currentTargets = db.GetVWCRM_MonthlyTargetPersonByGroupId(item.GroupId.Value, trans);
                    dbRes &= db.BulkDeleteCRM_MonthlyTargetPerson(currentTargets.ConvertType<CRM_MonthlyTargetPerson>(), trans);

                }

                dbRes &= db.BulkInsertCRM_MonthlyTargetPerson(insertList, trans);

            }

            if (dbRes.result)
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Aylık Hedef Değerleri Tanımlama işlemi başarıyla tamamlandı...", false, Url.Action("Index", "VWCRM_MonthlyTargetPerson", new { area = "CRM" }))
                }, JsonRequestBehavior.AllowGet);

            }

            trans.Rollback();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Error(dbRes.message)
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("Personel Aylık Hedefleri Güncelleme", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CRM_MonthlyTargetPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateCRM_MonthlyTargetPerson(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel Aylık Hedefleri Silme", SHRoles.CRMYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var datas = db.GetVWCRM_MonthlyTargetPersonByGroupId(id);
            var dbresult = db.BulkDeleteCRM_MonthlyTargetPerson(datas.ConvertType<CRM_MonthlyTargetPerson>());

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Personel Aylık Hedefleri başarıyla silindi...") : feedback.Error(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel Aylık Hedefleri Çekme", SHRoles.CRMYonetici)]
        public ContentResult GetTargetsByGroupId(Guid? groupId)
        {

            var db = new WorkOfTimeDatabase();

            if (groupId.HasValue)
            {
                var data = db.GetVWCRM_MonthlyTargetPersonByGroupId(groupId.Value);
                return Content(Infoline.Helper.Json.Serialize(data), "application/json");
            }

            return Content(Infoline.Helper.Json.Serialize(new VWCRM_MonthlyTargetPerson[0]), "application/json");

        }
    }
}
