using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Controllers
{

    



    public class VWPRJ_ProjectScopeLevelController : Controller
    {

        [PageInfo("Sözleşme bölgesel seviye tanımları Verileri", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectScopeLevel(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRJ_ProjectScopeLevelCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Sözleşme bölgesel seviye tanımları Verileri 2 ", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectScopeLevel(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Sözleşme arıza konusu seviye tanımı ekleme sayfası", SHRoles.ProjeYonetici)]
        public ActionResult Insert(VWPRJ_ProjectScopeLevelData model)
        {
            model.locationIds = new Guid[] { };
            return View(model);
        }

        [PageInfo("Sözleşme arıza konusu seviye tanımı ekleme sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VWPRJ_ProjectScopeLevelData item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var trans = db.BeginTransaction();
            var items = db.GetVWPRJ_ProjectScopeLevelItemsByProjectId(item.projectId ?? Guid.NewGuid());
            var scopes = items.Where(a => a.scopeLevelId != item.id && a.locationId.HasValue && item.locationIds.Contains(a.locationId.Value)).GroupBy(c => c.scopeLevelId);
            if (scopes.Count() > 0)
            {
                var text = string.Join("<br/>", scopes.Select(a => string.Join(",", a.Select(c => c.locationId_Title)) + " bölgeleri " + a.Select(c => c.scopeLevelId_Title).FirstOrDefault() + " tanımı için kullanılmış."));
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kaydetme işlemi başarısız. <br/>Hata Mesajı : " + text)
                }, JsonRequestBehavior.AllowGet);
            }



            var dbresult = db.InsertPRJ_ProjectScopeLevel(new PRJ_ProjectScopeLevel().B_EntityDataCopyForMaterial(item, true), trans);
            dbresult &= db.BulkInsertPRJ_ProjectScopeLevelItems(item.locationIds.Select(a => new PRJ_ProjectScopeLevelItems
            {
                created = DateTime.Now,
                createdby = userStatus.user.id,
                locationId = a,
                projectId = item.projectId,
                scopeLevelId = item.id
            }), trans);

            if (dbresult.result)
                trans.Commit();
            else
                trans.Rollback();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Sözleşme arıza konusu seviye tanımı güncelleme sayfası", SHRoles.ProjeYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = new VWPRJ_ProjectScopeLevelData().B_EntityDataCopyForMaterial(db.GetVWPRJ_ProjectScopeLevelById(id), true);
            data.locationIds = db.GetPRJ_ProjectScopeLevelItemsByScopeLevelId(data.id).Where(a => a.locationId.HasValue).Select(a => a.locationId.Value).ToArray();
            return View(data);
        }


        [PageInfo("Sözleşme arıza konusu seviye tanımı güncelleme sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VWPRJ_ProjectScopeLevelData item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var items = db.GetVWPRJ_ProjectScopeLevelItemsByProjectId(item.projectId ?? Guid.NewGuid());
            var scopes = items.Where(a => a.scopeLevelId != item.id && a.locationId.HasValue && item.locationIds.Contains(a.locationId.Value)).GroupBy(c => c.scopeLevelId);

            if (scopes.Count() > 0)
            {

                var text = string.Join("<br/>", scopes.Select(a => string.Join(",", a.Select(c => c.locationId_Title)) + " bölgeleri " + a.Select(c => c.scopeLevelId_Title).FirstOrDefault() + " tanımı için kullanılmış."));

                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kaydetme işlemi başarısız. <br/>Hata Mesajı : " + text)
                }, JsonRequestBehavior.AllowGet);
            }

            var trans = db.BeginTransaction();
            var dbresult = db.UpdatePRJ_ProjectScopeLevel(new PRJ_ProjectScopeLevel().B_EntityDataCopyForMaterial(item, true), true, trans);
            dbresult &= db.BulkDeletePRJ_ProjectScopeLevelItems(items.Where(a => a.scopeLevelId == item.id).Select(a => new PRJ_ProjectScopeLevelItems { id = a.id }).ToArray());
            dbresult &= db.BulkInsertPRJ_ProjectScopeLevelItems(item.locationIds.Select(a => new PRJ_ProjectScopeLevelItems
            {
                created = DateTime.Now,
                createdby = userStatus.user.id,
                locationId = a,
                projectId = item.projectId,
                scopeLevelId = item.id
            }), trans);

            if (dbresult.result)
                trans.Commit();
            else
                trans.Rollback();


            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Sözleşme bölgesel seviye tanımları Sil", SHRoles.ProjeYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var trans = db.BeginTransaction();
            var dbresult = db.DeletePRJ_ProjectScopeLevel(new PRJ_ProjectScopeLevel { id = id }, trans);
            dbresult &= db.BulkDeletePRJ_ProjectScopeLevelItems(db.GetPRJ_ProjectScopeLevelItemsByScopeLevelId(id).ToArray(), trans);

            if (dbresult.result)
                trans.Commit();
            else
                trans.Rollback();
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
