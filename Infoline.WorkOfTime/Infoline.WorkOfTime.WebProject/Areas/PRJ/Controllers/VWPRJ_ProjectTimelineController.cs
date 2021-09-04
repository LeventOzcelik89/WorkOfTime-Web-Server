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

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Controllers
{
	public class VWPRJ_ProjectTimelineController : Controller
    {
        [PageInfo("Proje Görev Raporu", SHRoles.ProjeYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var project = db.GetPRJ_Project();
            var projectTimeline = db.GetPRJ_ProjectTimeline();
            var model = new VMPageData();
            model.totalProject = project.Count();
            model.endProject = projectTimeline.Where(x => x.Status == (Int16)EnumPRJ_ProjectTimelineStatus.Bitti).Count();
            model.activeProjectCommission = projectTimeline.Where(x => x.Status == (Int16)EnumPRJ_ProjectTimelineStatus.Baslandi).Count();
            return View(model);
        }


        [PageInfo("Proje Zaman Çizelgesi Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectTimeline(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRJ_ProjectTimelineCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Proje Zaman Çizelgesi Detayı", SHRoles.ProjeYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectTimelineById(id);
            var persons = db.GetPRJ_ProjectTimelinePersonTimelineId(id);
            ViewBag.persons = persons.Select(x => x.IdUser);
            return View(data);
        }

        [PageInfo("Proje Zaman Çizelgesi Ekleme", SHRoles.ProjeYonetici)]
        public ActionResult Insert(Guid? projeId)
        {
            var end = DateTime.Now.AddMonths(1);
            var start = DateTime.Now;
            var data = new VWPRJ_ProjectTimeline
            {
                id = Guid.NewGuid(),
                StartDate = new DateTime(start.Year, start.Month, start.Day, 08, 30, 0),
                EndDate = new DateTime(end.Year, end.Month, end.Day, 18, 00, 0)
            };
            if (projeId.HasValue)
            {
                data.IdProject = projeId.Value;
                var db = new WorkOfTimeDatabase();
                var pro = db.GetPRJ_ProjectById(projeId.Value);
                if (pro != null)
                {
                    data.StartDate = new DateTime(pro.ProjectStartDate.Value.Year, pro.ProjectStartDate.Value.Month, pro.ProjectStartDate.Value.Day, 08, 30, 0);

                    if (pro.WarrantyEndDate.HasValue)
                        data.EndDate = new DateTime(pro.WarrantyEndDate.Value.Year, pro.WarrantyEndDate.Value.Month, pro.WarrantyEndDate.Value.Day, 18, 00, 0);
                    else
                        data.EndDate = new DateTime(pro.ProjectEndDate.Value.Year, pro.ProjectEndDate.Value.Month, pro.ProjectEndDate.Value.Day, 18, 00, 0); 
                }
            }
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Proje Zaman Çizelgesi Ekleme", SHRoles.ProjeYonetici)]
        public JsonResult Insert(PRJ_ProjectTimeline item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            if (string.IsNullOrEmpty(Request["IdPersons"]))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen personel seçimi gerçekleştirin..")
                }, JsonRequestBehavior.AllowGet);
            }

            var resultList = new ResultStatus();
            resultList.result = true;


            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var personList = Request["IdPersons"].Split(',').Select(x => new PRJ_ProjectTimelinePersons
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                IdProject = item.IdProject,
                IdTimeline = item.id,
                IdUser = x.ToGuid()
            });
            var trans = db.BeginTransaction();
            resultList &= db.InsertPRJ_ProjectTimeline(item);
            resultList &= db.BulkInsertPRJ_ProjectTimelinePersons(personList);

            if (resultList.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error("Satın alma ekleme işlemi başarısız.Lütfen daha sonra tekrar deneyiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            if (item.Type == (Int32)EnumPRJ_ProjectTimelineType.SatinAlma)
            {
                //Todo:Şahin
                //return Json(new ResultStatusUI
                //{
                //    Result = true,
                //    Object = Url.Action("Update", "VWINV_Procurement", new { area = "INV", id = satinAlma.id }),
                //    FeedBack = resultList.result ? feedback.Success("Satın alma talebi kaydı başarılı.Kalem bilgilerini gireceğiniz sayfa yeni sekmede açılmıştır.", false) : feedback.Error("Kaydetme işlemi başarısız")

                //}, JsonRequestBehavior.AllowGet);

                return null;
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = resultList.result ? feedback.Success("Gant şeması kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
                }, JsonRequestBehavior.AllowGet);
            }

        }


        [PageInfo("Proje Zaman Çizelgesi Güncelleme", SHRoles.ProjeYonetici)]
        public ActionResult Update(Guid id, bool delete = false)
        {
            var db = new WorkOfTimeDatabase();

            var data = db.GetVWPRJ_ProjectTimelineById(id);
            var persons = db.GetPRJ_ProjectTimelinePersonTimelineId(id);

            if (data.IdProject != null)
            {
                var pro = db.GetPRJ_ProjectById(data.IdProject);
                if (pro != null)
                {
                    //  data.StartDate = new DateTime(pro.ProjectStartDate.Value.Year, pro.ProjectStartDate.Value.Month, pro.ProjectStartDate.Value.Day, 08, 30, 0);
                    //  data.EndDate = new DateTime(pro.WarrantyEndDate.Value.Year, pro.WarrantyEndDate.Value.Month, pro.WarrantyEndDate.Value.Day, 08, 30, 0);
                }
            }


            ViewBag.persons = persons.Select(x => x.IdUser);
            ViewBag.Delete = delete;
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Proje Zaman Çizelgesi Güncelleme", SHRoles.ProjeYonetici)]
        public JsonResult Update(PRJ_ProjectTimeline item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var resulList = new ResultStatus();

            resulList.result = true;
            if (string.IsNullOrEmpty(Request["IdPersons"]))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen personel seçimi gerçekleştirin..")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;



            var trans = db.BeginTransaction();

            var persons = db.GetPRJ_ProjectTimelinePersonTimelineId(item.id);
            if (persons.Count() > 0)
            {
                resulList &= db.BulkDeletePRJ_ProjectTimelinePersons(persons);
            }

            var personList = Request["IdPersons"].Split(',').Select(x => new PRJ_ProjectTimelinePersons
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                IdProject = item.IdProject,
                IdTimeline = item.id,
                IdUser = x.ToGuid()
            });

            resulList &= db.BulkInsertPRJ_ProjectTimelinePersons(personList);
            resulList &= db.UpdatePRJ_ProjectTimeline(item);

            if (resulList.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Gant şeması güncellenirken sorun oluştu.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Gant şeması güncelleme işlemi başarılı")
                });
            }
        }


        [HttpPost]
        [PageInfo("Proje Zaman Çizelgesi Silme", SHRoles.ProjeYonetici)]
        public JsonResult Delete(string id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var timeLine = db.GetPRJ_ProjectTimelineById(Guid.Parse(id));
            var persons = db.GetPRJ_ProjectTimelinePersonTimelineId(Guid.Parse(id));
            var trans = db.BeginTransaction();
            var dbres1 = db.DeletePRJ_ProjectTimeline(timeLine);
            var dbres2 = db.BulkDeletePRJ_ProjectTimelinePersons(persons);

            if (!dbres1.result || !dbres2.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(dbres2.message, "Silme işlemi başarısız.Lütfen daha sonra tekrar deneyiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = dbres1.result && dbres2.result ? feedback.Success("Gant şeması silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Proje Zaman Çizelgesi Methodu", SHRoles.ProjeYonetici)]
        public List<TaskViewModel> ParentCat(VWPRJ_ProjectTimeline[] param)
        {
            var obj = new List<TaskViewModel>();

            var item = param.OrderBy(x => x.EndDate).ToList();
            var ilk = param.OrderBy(x => x.EndDate).FirstOrDefault() ?? new VWPRJ_ProjectTimeline();

            for (var i = 0; i < param.Count(); i++)
            {

                obj.Add(new TaskViewModel
                {
                    Title = item[i].Name,
                    TaskID = item[i].id,
                    ParentID = null,
                    Start = item[i].StartDate.Value,
                    End = item[i].EndDate.Value,
                    OrderId = i,
                    Summary = false,
                    Expanded = true,
                    Type = item[i].Type.Value,
                    Status = item[i].Status,
                    Status_Title= item[i].Status_Title
                });

            }

            return obj;
        }

        [PageInfo("Proje Zaman Çizelgesi Toplu Ekleme", SHRoles.ProjeYonetici)]
        public ActionResult Upsert(Guid IdProject)
        {
            return View(new VWPRJ_ProjectTimeline() { IdProject = IdProject });
        }

        [PageInfo("Proje Zaman Çizelgesi Önizleme", SHRoles.ProjePersonel)]
        public ActionResult Preview(Guid IdProject)
        {
            return View(new VWPRJ_ProjectTimeline() { IdProject = IdProject });
        }

        [PageInfo("Proje Gantt Şeması", SHRoles.ProjePersonel)]
        public JsonResult GanttBinder([DataSourceRequest] DataSourceRequest request, Guid projeId)
        {
            request.PageSize = 20;
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var c = db.GetVWPRJ_ProjectTimelineByIdProject(projeId).RemoveGeographies().OrderBy(s => s.StartDate).ToArray();
            var tasks = ParentCat(c).ToArray();

            return Json(tasks.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Proje Kilometre Taşları Zaman Çizelgesi",SHRoles.ProjePersonel)]
        public JsonResult Dependencies([DataSourceRequest] DataSourceRequest request, Guid projeId)
        {
            request.PageSize = 20;
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();

            var c = db.GetVWPRJ_ProjectTimeline(condition).Where(x => x.IdProject == projeId).RemoveGeographies().OrderBy(s => s.StartDate).ToArray();
            var tasks = ParentCat(c).ToArray();

            var dependencies = tasks.Select(item => new DependencyViewModel { DependencyID = item.OrderId, PredecessorID = item.ParentID, SuccessorID = item.TaskID, Type = item.Type }).ToList();

            return Json(dependencies.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Proje Zaman Çizelgesi Methodu", SHRoles.ProjePersonel)]
        public JsonResult Resources([DataSourceRequest] DataSourceRequest request, Guid projeId)
        {
            request.PageSize = 20;
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var c = db.GetVWPRJ_ProjectTimeline(condition).Where(x => x.IdProject == projeId).RemoveGeographies().OrderBy(s => s.StartDate).ToArray();
            var tasks = ParentCat(c).ToArray();
            var dependencies = tasks.Select(item => new { ID = item.TaskID, Name = item.Title, Color = item.Type == 1 ? "#f44336" : "#880e4f" }).ToList();
            return Json(dependencies.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}
