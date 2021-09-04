using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Controllers
{
	public class VWPRJ_ProjectTypeLevelController : Controller
    {
        [PageInfo("Sözleşme arıza konusu seviye tanımları Verileri", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectTypeLevel(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRJ_ProjectTypeLevelCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Sözleşme arıza konusu seviye tanımları Verileri 2 ", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRJ_ProjectTypeLevel(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Sözleşme arıza konusu seviye tanımı ekleme sayfası", SHRoles.ProjeYonetici)]
        public ActionResult Insert(VWPRJ_ProjectTypeLevel model)
        {
            return View(model);
        }

        [PageInfo("Sözleşme arıza konusu seviye tanımı ekleme sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VWPRJ_ProjectTypeLevel item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            if(!item.projectId.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Proje bulunamadı")
                }, JsonRequestBehavior.AllowGet);
            }
            if (!item.type.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Görev tipi bulunamadı")
                }, JsonRequestBehavior.AllowGet);
            }

            var control = db.GetVWPRJ_ProjectTypeLevelByProjectIdAndType(item.projectId.Value, item.type.Value);
            if(control != null)
            {
                   return Json(new ResultStatusUI
                   {
                       Result = false,
                       FeedBack = feedback.Warning("Daha önce aynı projeye aynı görev tipi için tanımlama yapılmış.")
                   }, JsonRequestBehavior.AllowGet);
            }
            
            var dbresult = db.InsertPRJ_ProjectTypeLevel(new PRJ_ProjectTypeLevel().B_EntityDataCopyForMaterial(item, true));

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
            var data = new VWPRJ_ProjectTypeLevel().B_EntityDataCopyForMaterial(db.GetVWPRJ_ProjectTypeLevelById(id), true);
            return View(data);
        }


        [PageInfo("Sözleşme arıza konusu seviye tanımı güncelleme sayfası", SHRoles.ProjeYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VWPRJ_ProjectTypeLevel item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            if (!item.projectId.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Proje bulunamadı")
                }, JsonRequestBehavior.AllowGet);
            }
            if (!item.type.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Görev tipi bulunamadı")
                }, JsonRequestBehavior.AllowGet);
            }
            var control = db.GetVWPRJ_ProjectTypeLevelByProjectIdAndTypeAndId(item.projectId.Value, item.type.Value,item.id);
            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Daha önce aynı projeye aynı görev tipi için tanımlama yapılmış.")
                }, JsonRequestBehavior.AllowGet);
            }
            var dbresult = db.UpdatePRJ_ProjectTypeLevel(new PRJ_ProjectTypeLevel().B_EntityDataCopyForMaterial(item, true));
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Sözleşme arıza konusu seviye tanımlı sil", SHRoles.ProjeYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbresult = db.DeletePRJ_ProjectTypeLevel(new PRJ_ProjectTypeLevel { id = id });
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
