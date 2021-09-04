using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_UserRoleController : Controller
    {
        [PageInfo("Kullanıcı Rol Methodu"), AllowEveryone]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_UserRole(condition).RemoveGeographies().OrderByDescending(c => c.created).ToDataSourceResult(request);
            data.Total = db.GetVWSH_UserRoleCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kullanıcı Rol Dropdown Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWSH_UserRole(condition);
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }

        [PageInfo("Rol Kullanıcı Yetkisi Ekleme", SHRoles.SistemYonetici)]
        public ActionResult Insert(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var userIds = db.GetSH_UserRoleByRoleId(id).Where(a => a.userid.HasValue).Select(a => a.userid.Value).ToArray();
            var data = new VMSH_UserRole { id = Guid.NewGuid(), roleid = id, userIds = userIds };
            return View(data);
        }

        [PageInfo("Rol Kullanıcı Yetkisi Ekleme", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMSH_UserRole item)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (item.roleid == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Rol Seçilmedi.")
                }, JsonRequestBehavior.AllowGet);
            }
            var userIds = db.GetSH_UserRoleByRoleId(item.roleid.Value);
            var trans = db.BeginTransaction();
            var rs = db.BulkDeleteSH_UserRole(userIds);
            if (item.userIds != null)
            {
                rs &= db.BulkInsertSH_UserRole(item.userIds.Select(a => new SH_UserRole
                {
                    roleid = item.roleid,
                    userid = a,
                    createdby = userStatus.user.id,
                    created = DateTime.Now,
                }), trans);
            }
            if (rs.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return Json(new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedback.Success("İşlem başarılı.") : feedback.Warning("İşlem başarısız.")
            }, JsonRequestBehavior.AllowGet); ;
        }
    }
}

