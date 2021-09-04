using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.APM.Controllers
{
	public class VWAPM_ActivityController : Controller
    {
        [PageInfo("Şirket Takvimi", SHRoles.Personel)]
        public ActionResult Index()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var userIds = userStatus.ChildPersons.Where(a => a.IdUser.HasValue).Select(a => a.IdUser.Value).ToList();
            userIds.Add(userStatus.user.id);
            var persons = db.GetSH_UserByIds(userIds.ToArray());
            ViewBag.generalType = (int)EnumAPM_ActivityGeneralType.Sirket;
            ViewBag.Title = "Şirket İçi Aktiviteler";
            return PartialView("~/Areas/APM/Views/VWAPM_Activity/Index.cshtml", persons);
        }



        [PageInfo("Müşteri Satış Aktiviteleri", SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
        public ActionResult IndexSales()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var userIds = new List<Guid> { userStatus.user.id };
            userIds.AddRange(db.GetSH_UserByRoleId(new Guid(SHRoles.CRMYonetici)).ToList());
            userIds.AddRange(db.GetSH_UserByRoleId(new Guid(SHRoles.SatisPersoneli)).ToList());
            userIds = userIds.GroupBy(a => a).Select(a => a.Key).ToList();
            if (!(userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici))))
            {
                userIds = userIds.Where(a => userStatus.ChildPersons.Select(c => c.IdUser).Contains(a)).ToList();
            }
            var model = db.GetSH_UserByIds(userIds.ToArray());
            ViewBag.generalType = (int)EnumAPM_ActivityGeneralType.Satis;
            ViewBag.Title = "Müşteri Satış Aktiviteleri";
            return PartialView("~/Areas/APM/Views/VWAPM_Activity/Index.cshtml", model);
        }



        [PageInfo("Aktiviteler Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWAPM_Activity(condition).ToDataSourceResult(request);
            data.Total = db.GetVWAPM_ActivityCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Aktiviteler Takvim Metodu", SHRoles.Personel)]
        public ContentResult DataSourceCalendar([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var activities = db.GetVWAPM_Activity(condition);
            var enumGeneric = EnumsProperties.EnumToArrayGeneric<EnumAPM_ActivityType>().ToArray();

            var data = activities.Select(a => new ActivityCalendarModel
            {
                color = enumGeneric.Where(c => c.Key == a.type.ToString()).Select(x => x.Generic["color"]).FirstOrDefault(),
                createdby = a.createdby.HasValue ? a.createdby.Value : System.UIHelper.Guid.Null,
                description = a.description,
                end = a.endDate,
                start = a.startDate,
                id = a.id,
                title = a.name,
                type = a.type,
                icon = enumGeneric.Where(c => c.Key == a.type.ToString()).Select(x => x.Generic["icon"]).FirstOrDefault()
            });

            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Aktiviteler Dropdown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWAPM_Activity(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Aktiviteler Miktar DataSource", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWAPM_ActivityCount(condition.Filter);
            return count;
        }

        [AllowEveryone]
        [PageInfo("Aktiviteler Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDownForRelations([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWAPM_ActivityRelationTables(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }



        [PageInfo("Aktivite Detay", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var data = new VMAPM_ActivityModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Aktivite Ekleme Sayfası", SHRoles.Personel)]
        public ActionResult Insert(VMAPM_ActivityModel model)
        {
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Aktivite Ekleme Metodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMAPM_ActivityModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Aktivite Düzenleme Sayfası", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var data = new VMAPM_ActivityModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Aktivite Düzenleme Metodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMAPM_ActivityModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Aktivite Silme Metodu", SHRoles.Personel)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var data = new VMAPM_ActivityModel { id = id }.Load();
            var dbresult = data.Delete();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
