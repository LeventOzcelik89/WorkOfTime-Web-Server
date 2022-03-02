using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.PRJ.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Controllers
{
    public class VWPRJ_ProjectController : Controller
    {
        [PageInfo("Tüm dProjeler", SHRoles.ProjeYonetici)]  //TODO [FF] Rol düzenlenecek
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectPageReportSummary() ?? new VWPRJ_ProjectPageReport();
            return View(data);
        }
        [PageInfo("Görev Aldığım Projeler", SHRoles.ProjePersonel)]
        public ActionResult MyIndex()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var dataGrid = new PRJ_ProjectPersonDepartmentsModel().GetMyProject(userStatus.user.id);
            ViewBag.DataSource = dataGrid;
            return View(dataGrid);
        }

        [PageInfo("Projeler methodu", SHRoles.Personel,SHRoles.HakEdisBayiPersoneli)]
        public JsonResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_Project(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRJ_ProjectCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Projeler Miktar DataSource", SHRoles.Personel, SHRoles.HakEdisBayiPersoneli)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWPRJ_ProjectCount(condition.Filter);
            return count;
        }

        [PageInfo("Projeler Veri Methodu", SHRoles.Personel, SHRoles.HakEdisBayiPersoneli)]
        public JsonResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetPRJ_Project(condition).Select(c => new { Id = c.id, CorporationId = c.CorporationId, ProjectName = c.ProjectName, ProjectStartDate = c.ProjectStartDate, ProjectEndDate = c.ProjectEndDate });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("FTM_TaskProject Projeler Veri Methodu", SHRoles.Personel)]
        public JsonResult DataSourceDropDownOfFTMTask([DataSourceRequest] DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWPRJ_Project(condition);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Projelerin detay ekranı", SHRoles.ProjePersonel)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMPRJ_ProjectModel();
            model.B_EntityDataCopyForMaterial(db.GetVWPRJ_ProjectById(id));

            model.taskIds = new List<Guid?>();

            if (model.CorporationId.HasValue)
            {
                var tasks = db.GetFTM_TaskByCustomerId(model.CorporationId.Value);
                model.taskIds = tasks.Select(x => (Guid?)x.id).Take(100).ToList();
            }

            if (model.ProjectTechnicalType != null)
            {
                ViewBag.teknik = model.ProjectTechnicalType.Split(',').ToArray();
            }

            return View(model);
        }
        [PageInfo("Proje Ekleme", SHRoles.ProjeYonetici)]
        public ActionResult Insert(Guid? companyId)
        {
            DateTime d = DateTime.Now;
            var data = new VWPRJ_Project
            {
                id = Guid.NewGuid(),
                CompanyId = companyId,
                ProjectStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ProjectCode = d.Year.ToString() + d.Month.ToString() + d.Day.ToString() + d.Hour.ToString() + d.Minute.ToString() + d.Millisecond.ToString()
            };
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Proje Ekleme", SHRoles.ProjeYonetici)]
        public JsonResult Insert(PRJ_Project item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            item.ProjectTechnicalType = Request["ProjeTeknikTip"].ToString();

            var projectSponsorId = new Guid(Request["ProjectSponsorId"]);
            var projectManagerId = new Guid(Request["ProjectManagerId"]);


            var SponsorDep = new INV_CompanyDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = item.createdby,
                Name = "Proje Sponsoru",
                PID = null,
                ProjectId = item.id,
                Type = (int)EnumINV_CompanyDepartmentsType.Matrix
            };

            var ManagerDep = new INV_CompanyDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = item.createdby,
                Name = "Proje Yöneticisi",
                PID = SponsorDep.id,
                ProjectId = item.id,
                Type = (int)EnumINV_CompanyDepartmentsType.Matrix
            };

            var SponsorPerson = new INV_CompanyPersonDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                IdUser = projectSponsorId,
                Title = "Proje Sponsoru",
                StartDate = item.ProjectStartDate,
                EndDate = item.ProjectEndDate,
                IsBasePosition = true,
                DepartmentId = SponsorDep.id,
            };

            var ManagerPerson = new INV_CompanyPersonDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                IdUser = projectManagerId,
                Title = "Proje Yöneticisi",
                StartDate = item.ProjectStartDate,
                EndDate = item.ProjectEndDate,
                Manager1 = projectSponsorId,
                IsBasePosition = true,
                DepartmentId = ManagerDep.id,
            };

            var resultList = new List<ResultStatus>();
            var trans = db.BeginTransaction();
            resultList.Add(db.InsertPRJ_Project(item, trans));
            resultList.Add(db.InsertINV_CompanyDepartments(SponsorDep, trans));
            resultList.Add(db.InsertINV_CompanyDepartments(ManagerDep, trans));
            resultList.Add(db.InsertINV_CompanyPersonDepartments(SponsorPerson, trans));
            resultList.Add(db.InsertINV_CompanyPersonDepartments(ManagerPerson, trans));


            if (resultList.Count(a => a.result == false) > 0)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(String.Join("\n", resultList.Select(a => a.message)), "Proje kaydetme işlemi başarız oldu. Lütfen daha sonra tekrar deneyiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Proje ekleme işlemi başarılı.Diğer bilgileri düzenleme sayfasından yapabilirsiniz.", false, Url.Action("Update", "VWPRJ_Project", new { area = "PRJ", id = item.id }))
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [PageInfo("Proje Güncelleme", SHRoles.ProjeYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMPRJ_ProjectModel();
            model.B_EntityDataCopyForMaterial(db.GetVWPRJ_ProjectById(id));

            model.taskIds = new List<Guid?>();
            var companyDepartments = db.GetINV_CompanyDepartmentByProjectId(id);
			if (companyDepartments.Count() >0)
			{
                var sponsor = companyDepartments.Where(x => x.Name == "Proje Sponsoru").FirstOrDefault();
				if (sponsor != null)
				{
                    var sponsorPerson = db.GetINV_CompanyPersonDepartmentsByDepartmentId(sponsor.id);
					if (sponsorPerson != null)
					{
                        model.ProjectSponsorId = sponsorPerson.IdUser;
					}
				}
                var manager = companyDepartments.Where(x => x.Name == "Proje Yöneticisi").FirstOrDefault();

                if (manager != null)
				{
                    var managerPerson = db.GetINV_CompanyPersonDepartmentsByDepartmentId(manager.id);
                    if (managerPerson != null)
                    {
                        model.ProjectManagerId = managerPerson.IdUser;
                    }

                }

            }

            if (model.CorporationId.HasValue)
            {
                var tasks = db.GetFTM_TaskByCustomerId(model.CorporationId.Value);
                model.taskIds = tasks.Select(x => (Guid?)x.id).ToList();
            }

            if (model != null && model.ProjectCode != null &&
               (model.ProjectCode.IndexOf("AKL_", StringComparison.Ordinal) == 0 || model.ProjectCode.IndexOf("INF_", StringComparison.Ordinal) == 0))
            {
                ViewBag.ProjectCode = model.ProjectCode;

                model.ProjectCode = model.ProjectCode.Substring(10, model.ProjectCode.Length - 10);
            }
            if (model.ProjectTechnicalType != null)
            {
                ViewBag.teknik = model.ProjectTechnicalType.Split(',').ToArray();
            }
            return View(model);

        }
        [PageInfo("Proje Güncelleme", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(PRJ_Project item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var resultList = new List<ResultStatus>();
            var trans = db.BeginTransaction();
            var proje = db.GetPRJ_ProjectById(item.id);

            var projectSponsorId = new Guid(Request["ProjectSponsorId"]);
            var projectManagerId = new Guid(Request["ProjectManagerId"]);


            var sponsor = db.GetINV_CompanyDepartmentsByProjectId(item.id);
			var manager = db.GetINV_CompanyPersonDepartmentsByDepartmentId(sponsor.id);
            var manager2 = db.GetINV_CompanyPersonDepartmentsByDepartmentId(manager.id);
            db.DeleteINV_CompanyDepartments(sponsor);
			if (manager != null)
			{
                db.DeleteINV_CompanyPersonDepartments(manager);
            }
			if (manager2 != null)
			{
                db.DeleteINV_CompanyPersonDepartments(manager2);
            }

            var SponsorDep = new INV_CompanyDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = item.createdby,
                Name = "Proje Sponsoru",
                PID = null,
                ProjectId = item.id,
                Type = (int)EnumINV_CompanyDepartmentsType.Matrix
            };

            var ManagerDep = new INV_CompanyDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = item.createdby,
                Name = "Proje Yöneticisi",
                PID = SponsorDep.id,
                ProjectId = item.id,
                Type = (int)EnumINV_CompanyDepartmentsType.Matrix
            };

            var SponsorPerson = new INV_CompanyPersonDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                IdUser = projectSponsorId,
                Title = "Proje Sponsoru",
                StartDate = item.ProjectStartDate,
                EndDate = item.ProjectEndDate,
                IsBasePosition = true,
                DepartmentId = SponsorDep.id,
            };

            var ManagerPerson = new INV_CompanyPersonDepartments
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                IdUser = projectManagerId,
                Title = "Proje Yöneticisi",
                StartDate = item.ProjectStartDate,
                EndDate = item.ProjectEndDate,
                Manager1 = projectSponsorId,
                IsBasePosition = true,
                DepartmentId = ManagerDep.id,
            };

            resultList.Add(db.InsertINV_CompanyDepartments(SponsorDep, trans));
            resultList.Add(db.InsertINV_CompanyDepartments(ManagerDep, trans));
            resultList.Add(db.InsertINV_CompanyPersonDepartments(SponsorPerson, trans));
            resultList.Add(db.InsertINV_CompanyPersonDepartments(ManagerPerson, trans));


            if (userStatus.AuthorizedRoles.Contains(new Guid("d55ac8fa-327e-48ed-aee0-d875bccfd868")) && proje.createdby != userStatus.user.id)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Sadece kendi oluşturduğunuz proejelerde güncelleme işlemi gerçekleştirebilirsiniz.") }, JsonRequestBehavior.AllowGet);
            }
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            if (item.ProjectTechnicalType == null)
            {
                item.ProjectTechnicalType = Request["ProjeTeknikTip"] != "" ? Request["ProjeTeknikTip"].ToString() : "";
            }

            resultList.Add(db.UpdatePRJ_Project(item, false, trans));


            if (resultList.Count(a => a.result == false) > 0)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(String.Join("\n", resultList.Select(a => a.message)), "Proje güncelleme işlemi başarız oldu. Lütfen daha sonra tekrar deneyiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            var result = new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Proje Güncelleme İşlemi Başarılı", false, (Request.UrlReferrer.AbsolutePath == "/PRJ/PRJ_Project/Update" ? Url.Action("Index", "PRJ_Project") : null))
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Proje Silme Methodu", SHRoles.ProjeYonetici)]
        [HttpPost]
        public JsonResult Delete(PRJ_Project item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var proje = db.GetPRJ_ProjectById(item.id);

            if (userStatus.AuthorizedRoles.Contains(new Guid("d55ac8fa-327e-48ed-aee0-d875bccfd868")) && proje.createdby != userStatus.user.id)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Sadece kendi oluşturduğunuz projeleri silebilirsiniz.") }, JsonRequestBehavior.AllowGet);
            }

            var resultList = new List<ResultStatus>();
            var trans = db.BeginTransaction();
            var myDepartments = db.GetINV_CompanyDepartmentByProjectId(item.id);
            var myPersons = db.GetINV_CompanyPersonDepartmentsInDepartmentId(myDepartments.Select(a => a.id).ToArray());
            var myPersonsAvilibity = db.GETINV_CompanyPersonAvailabilityByProjectId(item.id);
            var projectTimelines = db.GetPRJ_ProjectTimelineByIdProject(item.id);

            resultList.Add(db.BulkDeleteINV_CompanyPersonDepartments(myPersons, trans));
            resultList.Add(db.BulkDeleteINV_CompanyDepartments(myDepartments, trans));
            resultList.Add(db.BulkDeleteINV_CompanyPersonAvailability(myPersonsAvilibity, trans));
            resultList.Add(db.BulkDeletePRJ_ProjectTimeline(projectTimelines, trans));
            resultList.Add(db.DeletePRJ_Project(item, trans));


            var count = resultList.Count(a => a.result == false);
            if (count == 0)
            {
                trans.Commit();
            }
            else
            {
                trans.Commit();
            }

            return Json(new ResultStatusUI
            {
                Result = count == 0,
                FeedBack = count == 0 ? feedback.Success("Proje Silme Durumuna Alındı") : feedback.Error("İşlem Başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Proje Maaliyetleri", SHRoles.ProjeYonetici)]
        public ActionResult Cost(Guid IdProject)
        {
            var model = new PRJ_ProjectCostModel(IdProject).Calculate();
            ViewBag.ProjeId = IdProject;
            var projectName = new WorkOfTimeDatabase().GetPRJ_ProjectById(IdProject);
            ViewBag.ProjectName = projectName.ProjectName;
            return View(model);
        }

        [PageInfo("Proje Raporları", SHRoles.ProjeYonetici)] //TODO [FF] roller düzeltilecek 
        public ActionResult Dashboard()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectPageReportSummary() ?? new VWPRJ_ProjectPageReport();
            return View(data);
        }
        [PageInfo("Proje Grafikleri Metodu", SHRoles.ProjeYonetici)]
        public ContentResult DashboardResult()
        {
            var db = new WorkOfTimeDatabase();
            var projects = db.GetVWPRJ_Project();
            var result = new ResultStatusUI
            {
                Result = true,
                Object = new
                {
                    Projects = projects,
                }
            };
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }
    }
}
