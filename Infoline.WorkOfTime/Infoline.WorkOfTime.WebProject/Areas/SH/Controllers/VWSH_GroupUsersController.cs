using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH
{
    public class VWSH_GroupUsersController : Controller
    {
        [PageInfo("Grup & Ekip Tanımlamaları", SHRoles.Personel)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Grup & Ekip Tanımlamaları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_GroupUsers(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_GroupUsersCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Grup & Ekip Tanımlamaları Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_GroupUsers(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Grup & Ekip Tanımlamaları Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_GroupUsersById(id);
            return View(data);
        }

        [PageInfo("Grup & Ekip Tanımlamaları Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VMSH_GroupUsersModel item)
        {
            var data = item.Load();
            return View(data);
        }

        [PageInfo("Grup & Ekip Tanımlamaları Methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMSH_GroupUsersModel item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var userControl = db.GetVWSH_GroupUsersByUserIds(item.groupUserIds);

            if (userControl != null && userControl.Count() > 0)
            {
                var users = userControl.Select(x => x.userId_Title).ToList();
                var data = string.Join(", ", users) + " zaten bir ekibe kayıtlıdır.";

                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning(data)
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = item.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [PageInfo("Grup & Ekip Tanımlamaları Güncelleme", SHRoles.Personel)]
        public ActionResult Update(VMSH_GroupUsersModel item)
        {
            var data = item.Load();
            return View(data);
        }

        [PageInfo("Grup & Ekip Tanımlamaları Güncelleme Methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMSH_GroupUsersModel item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = item.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Grup & Ekip Tanımlamaları Silme", SHRoles.Personel)]
        [HttpPost]
        public JsonResult Delete(VMSH_GroupUsersModel item)
        {
            return Json(new ResultStatusUI(item.Delete()), JsonRequestBehavior.AllowGet);

        }
    }
}
