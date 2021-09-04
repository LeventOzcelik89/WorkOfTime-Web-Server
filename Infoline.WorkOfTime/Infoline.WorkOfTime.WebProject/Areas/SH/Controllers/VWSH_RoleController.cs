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
    public class VWSH_RoleController : Controller
    {
        [PageInfo("Rol Tanımları",SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Rol Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_Role(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_RoleCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Rol Dropdown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_Role(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Rol Adet Metodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWSH_RoleCount(condition.Filter);
            return count;
        }

        [PageInfo("Rol Detayı", SHRoles.SistemYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_RoleById(id);
            return View(data);
        }

        [PageInfo("Rol Tanımlama", SHRoles.SistemYonetici)]
        public ActionResult Insert()
        {
            var data = new VWSH_Role { id = Guid.NewGuid() };
            return View(data);
        }


        [PageInfo("Rol Tanımlama", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_Role item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.roletype = (int)EnumSH_RolesRoleType.userdefine;
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertSH_Role(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Rol Güncelleme", SHRoles.SistemYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetSH_RoleById(id);
            return View(model);
        }


        [PageInfo("Rol Güncelleme", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_Role item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var rs = db.UpdateSH_Role(item, false);

            return Json(new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Rol Kopyalama", SHRoles.SistemYonetici)]
        public ActionResult UpdateCopy(Guid id)
        {
            var data = new SH_Role { id = id };
            return View(data);
        }

        [PageInfo("Rol Kopyalama Methodu", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult UpdateCopy(SH_Role item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var control = db.GetSH_RoleByName(item.rolname);
            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı rol ismi mevcut.")
                }, JsonRequestBehavior.AllowGet);
            }

            var pagesRoles = db.GetSH_PagesRoleByRoleId(item.id);
            var filesRole = db.GetSH_FilesRoleByRoleId(item.id);

            var trans = db.BeginTransaction();
            var newRole = new SH_Role
            {
                roletype = (Int32)EnumSH_RolesRoleType.userdefine,
                rolname = item.rolname,
                createdby = userStatus.user.id,
                roledescription = item.roledescription,
            };

            var dbres = db.InsertSH_Role(newRole, trans);

            dbres &= db.BulkInsertSH_PagesRole(pagesRoles.Select(x => new SH_PagesRole
            {
                createdby = userStatus.user.id,
                roleid = newRole.id,
                action = x.action
            }), trans);

            dbres &= db.BulkInsertSH_FilesRole(filesRole.Select(x => new SH_FilesRole
            {
                createdby = userStatus.user.id,
                roleid = newRole.id,
                dataTable = x.dataTable,
                delete = x.delete,
                fileGroup = x.fileGroup,
                insert = x.insert,
                preview = x.preview,
            }), trans);

            if (!dbres.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = dbres.result,
                    FeedBack = feedback.Warning("Rol kopyalama işlemi başarısız oldu." + dbres.message)
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = feedback.Success("Rol kopyalama işlemi başarı ile gerçekleşti.")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Rol Silme İşlemi", SHRoles.SistemYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var resultList = new List<ResultStatus>();
            var role = new SH_Role { id = id };
            var roleUser = db.GetSH_UserRoleByRoleId(id);
            var rolePage = db.GetSH_PagesRoleByRoleId(id);
            var roleFile = db.GetSH_FilesRoleByRoleId(id);

            resultList.Add(db.DeleteSH_Role(role));
            resultList.Add(db.BulkDeleteSH_UserRole(roleUser));
            resultList.Add(db.BulkDeleteSH_PagesRole(rolePage));
            resultList.Add(db.BulkDeleteSH_FilesRole(roleFile));

            var result = new ResultStatusUI
            {
                Result = resultList.Count(a => a.result == false) == 0,
                FeedBack = resultList.Count(a => a.result == false) == 0 ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
