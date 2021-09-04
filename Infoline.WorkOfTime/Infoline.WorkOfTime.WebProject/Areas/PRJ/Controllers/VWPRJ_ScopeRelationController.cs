using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Controllers
{
	public class VWPRJ_ScopeRelationController : Controller
    {
        [PageInfo("Kapsam Tanımları Listesi Verileri", SHRoles.ProjeYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ScopeRelation(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRJ_ScopeRelationCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Kapsam Tanımları Listesi Verileri 2", SHRoles.ProjeYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ScopeRelation(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Kapsam Tanımları Ekleme Sayfası", SHRoles.ProjeYonetici)]
        public ActionResult Insert(VWPRJ_ScopeRelation model)
        {
            var project = new WorkOfTimeDatabase().GetPRJ_ProjectById(model.projectId.Value);
            model.startDate = project.ProjectStartDate;
            model.endDate = project.ProjectEndDate;
            model.corporateId = project.CorporationId;
            return View(model);
        }

        [PageInfo("Kapsam Tanımları Ekleme Sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(PRJ_ScopeRelation item, Guid[] fixtureIds, Guid[] storageIds)
        {
            var db = new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = false };
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var project = db.GetPRJ_ProjectById(item.projectId.Value);
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            if (item.startDate == null || item.startDate < project.ProjectStartDate)
            {
                item.startDate = project.ProjectStartDate;
            }
            if (item.endDate == null || item.endDate > project.ProjectEndDate)
            {
                item.endDate = project.ProjectEndDate;
            }
            
            if (fixtureIds != null && fixtureIds.Count() > 0)
            {
                dbresult = db.BulkInsertPRJ_ScopeRelation(fixtureIds.Select(a => new PRJ_ScopeRelation
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = item.createdby,
                    startDate = item.startDate,
                    endDate = item.endDate,
                    corporateId = item.corporateId,
                    projectId = item.projectId,
                    inventoryId = a,
                    storageId = item.storageId
                }));
            }
            else
            {
                dbresult = db.InsertPRJ_ScopeRelation(item);
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kapsam Tanımları Güncelleme Sayfası", SHRoles.ProjeYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ScopeRelationById(id);
            return View(data);
        }

        [PageInfo("Kapsam Tanımları Güncelleme Sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(PRJ_ScopeRelation item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var project = db.GetPRJ_ProjectById(item.projectId.Value);
            var row = db.GetPRJ_ScopeRelationById(item.id);
            row.changed = DateTime.Now;
            row.changedby = userStatus.user.id;

            if (item.startDate == null || item.startDate < project.ProjectStartDate)
            {
                row.startDate = project.ProjectStartDate;
            }
            else
            {
                row.startDate = item.startDate;
            }

            if (item.endDate == null || item.endDate > project.ProjectEndDate)
            {
                row.endDate = project.ProjectEndDate;
            }
            else
            {
                row.endDate = item.endDate;
            }

            var dbresult = db.UpdatePRJ_ScopeRelation(row, true);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kapsam Tanımları Silme İşlemi", SHRoles.ProjeYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new PRJ_ScopeRelation { id = new Guid(a) });

            var dbresult = db.BulkDeletePRJ_ScopeRelation(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
