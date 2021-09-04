using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
    public class VWINV_CompanyPersonDepartmentsController : Controller
    {
        [PageInfo("Şirket Personel Pozisyonları Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonDepartments(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CompanyPersonDepartmentsCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Şirket Personel Pozisyonları Methodu", SHRoles.Personel)]
        public JsonResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWINV_CompanyPersonDepartments(condition).Where(x => x.Person_Title != null);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Şirket personel pozisyonlarının detayı", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonDepartmentsById(id);
            return View(data);
        }

        [PageInfo("Şirket Personel Pozisyonları Ekleme", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? DepartmentId, Guid? IdUser)
        {
            var db = new WorkOfTimeDatabase();
            var data = new VWINV_CompanyPersonDepartments
            {
                id = Guid.NewGuid(),
                IdUser = IdUser,
            };

            if (DepartmentId.HasValue)
            {
                data.DepartmentId = DepartmentId.Value;

                var departman = db.GetINV_CompanyDepartmentsById(DepartmentId.Value);

                if (departman != null)
                {
                    data.ProjectId = departman.ProjectId;
                    if (data.ProjectId != null)
                    {
                        var project = db.GetPRJ_ProjectById(data.ProjectId.Value);
                        data.StartDate = project.ProjectStartDate;
                        data.EndDate = project.ProjectEndDate;
                    }
                }
            }

            return View(data);
        }


        [PageInfo("Şirket Personel Pozisyonları Ekleme", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(INV_CompanyPersonDepartments item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var departman = db.GetVWINV_CompanyDepartmentsById(item.DepartmentId.Value);


            if (departman == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Seçilen departman silinmiş...!")
                }, JsonRequestBehavior.AllowGet);
            }


            if (item.IsBasePosition == true)
            {
                var positions = db.GetVWINV_CompanyPersonDepartmentsActiveBasePositions(item.IdUser.Value);
                var basePosition = positions.Where(a => a.ProjectId == departman.ProjectId && a.OrganizationType == departman.Type);

                if (basePosition.Count() > 0)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning("Bu personelin asıl görevi bu şemada zaten mevcuttur.Asıl görev veremezsiniz.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            if (departman.ProjectId.HasValue)
            {
                var project = db.GetPRJ_ProjectById(departman.ProjectId.Value);
                if (project != null)
                {
                    item.StartDate = project.ProjectStartDate;
                    item.EndDate = project.ProjectEndDate;
                }
            }



            var trans = db.BeginTransaction();
            var dbres = new ResultStatus { result = true };

            if (item.IdUser.HasValue && departman.Type == (Int32)(EnumINV_CompanyDepartmentsType.Matrix))
            {
                var isProjectPersonel = db.GetVWSH_UserRoleByUserId(item.IdUser.Value).Where(x => x.roleid == Guid.Parse(SHRoles.ProjePersonel)).Any();
                if (!isProjectPersonel)
                {
                    dbres &= db.InsertSH_UserRole(new SH_UserRole
                    {
                        created = item.created,
                        createdby = userStatus.user.id,
                        roleid = Guid.Parse(SHRoles.ProjePersonel),
                        userid = item.IdUser.Value,
                    }, trans);
                }
            }

            dbres &= db.InsertINV_CompanyPersonDepartments(item, trans);
            if (dbres.result == true)
            {
                trans.Commit();
                new ManagersCalculator().Run();
            }
            else
            {
                trans.Rollback();
            }

            var result = new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result
                    ? feedback.Success("Personeli Pozisyona atama işlemi başarı ile gerçekleşti.")
                    : feedback.Error(dbres.message)
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Şirket personel pozisyonlarını güncelleme ekranı", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonDepartmentsById(id);
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Şirket Personel Pozisyonları Güncelleme", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public JsonResult Update(INV_CompanyPersonDepartments item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();



            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;


            if (item.IsBasePosition == true)
            {
                var departman = db.GetINV_CompanyDepartmentsById(item.DepartmentId.Value);

                if (departman.ProjectId != null)
                {
                    var project = db.GetPRJ_ProjectById(departman.ProjectId.Value);

                    if (item.StartDate > project.ProjectEndDate || item.EndDate > project.ProjectEndDate)
                    {
                        return Json(new ResultStatusUI
                        {
                            Result = false,
                            FeedBack = feedback.Warning("Seçmiş olduğunuz başlangıç ve bitiş tarihleri proje bitiş tarihinden sonrası olamaz.")
                        });
                    }

                    if (item.StartDate < project.ProjectStartDate || item.EndDate < project.ProjectStartDate)
                    {
                        return Json(new ResultStatusUI
                        {
                            Result = false,
                            FeedBack = feedback.Warning("Seçmiş olduğunuz başlangıç ve bitiş tarihleri proje başlangıç tarihinden öncesi olamaz.")
                        });
                    }
                }

                var positions = db.GetVWINV_CompanyPersonDepartmentsActiveBasePositions(item.IdUser.Value);
                var basePosition = positions.Where(a => a.ProjectId == departman.ProjectId && a.OrganizationType == departman.Type && a.id != item.id);

                if (basePosition.Count() > 0)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning("Bu personelin asıl görevi bu şemada zaten mevcuttur.Asıl görev veremezsiniz.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            var dbresult = db.UpdateINV_CompanyPersonDepartments(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            new ManagersCalculator().Run();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Benim Projelerimin Sayısı", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ContentResult MyProjectCount()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = db.GetVWINV_CompanyPersonDepartmentsByIdUser(userStatus.user.id);
            var veri = data.Where(x => x.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Matrix);
            return Content(Infoline.Helper.Json.Serialize(veri.Count()), "application/json");

        }


        [HttpPost]
        [PageInfo("Şirket Personel Pozisyonlarını Silme", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var person = db.GetINV_CompanyPersonDepartmentsById(id);
            var dbResult = new ResultStatus { result = false };
            if (person != null)
            {
                if (person.created > DateTime.Now.AddDays(-1))
                {
                    var userRole = db.GetSH_UserRoleByUserIdRoleId((Guid)person.IdUser, Guid.Parse(SHRoles.ProjePersonel));
                    dbResult = db.DeleteINV_CompanyPersonDepartments(person);
                    dbResult = db.BulkDeleteSH_UserRole(userRole.Where(a => a.created == person.created));
                }
                else
                {
                    person.EndDate = DateTime.Now;
                    dbResult = db.UpdateINV_CompanyPersonDepartments(person);
                }
            }

            var result = new ResultStatusUI
            {
                Object = id,
                Result = dbResult.result,
                FeedBack = dbResult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            };

            new ManagersCalculator().Run();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}