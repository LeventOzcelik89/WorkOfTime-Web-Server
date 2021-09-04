using Infoline.Framework.Helper;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Controllers
{
    public class VWPRJ_ProjectServiceLevelController : Controller
    {
        [PageInfo("Sözleşme Hizmet Seviye Tanımları Ekleme Sayfası", SHRoles.ProjeYonetici)]
        public ActionResult Upsert(Guid projectId)
        {

            var db = new WorkOfTimeDatabase();
            var serviceLevels = db.GetVWPRJ_ProjectServiceLevelByProjectId(projectId);
            var typeLevels = db.GetVWPRJ_ProjectTypeLevelByProjectId(projectId);
            var scopeLevels = db.GetVWPRJ_ProjectScopeLevelScopeLevelByContractId(projectId);
            var model = new List<VWPRJ_ProjectServiceLevel>();


            foreach (var scopLevel in scopeLevels)
            {
                foreach (var typeLevel in typeLevels)
                {
                    var item = serviceLevels.Where(a => a.scopeLevelId == scopLevel.id && a.taskType == typeLevel.type).FirstOrDefault() ?? new VWPRJ_ProjectServiceLevel
                    {
                        projectId = projectId,
                        scopeLevelId = scopLevel.id,
                        scopeLevelId_Title = scopLevel.level,
                        taskType = typeLevel.type,
                        type_Title = typeLevel.type_Title
                    };
                    item.Level_Title = typeLevel.level;
                    item.resolutionType = item.resolutionType ?? (int)EnumPRJ_ProjectServiceLevelResolutionType.Gun;
                    item.resolutionType_Title = item.resolutionType_Title ?? "Gün";
                    item.resolutionTime = item.resolutionTime ?? 1;
                    item.startTime = item.startTime ?? 8;
                    item.endTime = item.endTime ?? 17;
                    item.amercement = item.amercement ?? 0;
                    item.isWorkingTime = item.isWorkingTime ?? true;
                    model.Add(item);
                }
            }
            return View(model.ToArray());
        }

        [PageInfo("Sözleşme Hizmet Seviye Tanımları Ekleme Sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Upsert(VWPRJ_ProjectServiceLevel[] items)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var deleteServiceLevels = db.GetPRJ_ProjectServiceLevelByContractId(items.Where(a => a.projectId.HasValue).Select(a => a.projectId.Value).FirstOrDefault());
            var insertServiceLevels = items.Select(g => new PRJ_ProjectServiceLevel().EntityDataCopyForMaterial(g, true));

            var trans = db.BeginTransaction();
            var dbresult = db.BulkDeletePRJ_ProjectServiceLevel(deleteServiceLevels, trans);
            dbresult &= db.BulkInsertPRJ_ProjectServiceLevel(insertServiceLevels, trans);

            if (dbresult.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Sözleşme Hizmet Seviye Tanımları Verileri")]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectServiceLevel(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRJ_ProjectServiceLevelCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Sözleşme Hizmet Seviye Tanımları Verileri 2 ")]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectServiceLevel(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}
