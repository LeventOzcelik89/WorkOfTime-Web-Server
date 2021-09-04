using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.DOC.Controllers
{
    public class VWDOC_DocumentController : Controller
    {
        [PageInfo("Dokümanlar", SHRoles.DokumanYonetimRolu)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Doküman Yönetimi Adet Metodu", SHRoles.DokumanYonetimRolu)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            return db.GetVWDOC_DocumentCount(condition.Filter);
        }

        [PageInfo("Doküman Yönetimi Veri Methodu", SHRoles.DokumanYonetimRolu)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWDOC_Document(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWDOC_DocumentCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Doküman Yönetim Dropdown Methodu", SHRoles.DokumanYonetimRolu)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWDOC_Document(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Doküman Yönetim Detayı", SHRoles.DokumanYonetimRolu)]
        public ActionResult Detail(VMDOC_DocumentModel model)
        {
            return View(model.Load());
        }

        [PageInfo("Doküman Ekleme", SHRoles.DokumanYonetimRolu)]
        public ActionResult Insert(VMDOC_DocumentModel data)
        {
            data.id = Guid.NewGuid();
            return View(data.Load());
        }

        [PageInfo("Doküman Ekleme Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Insert(VMDOC_DocumentModel item, bool isPost = true)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Doküman Düzenleme", SHRoles.DokumanYonetimRolu)]
        public ActionResult Update(VMDOC_DocumentModel item)
        {
            var data = item.Load();
            return View(data);
        }

        [PageInfo("Revizyon Talebi", SHRoles.DokumanYonetimRolu)]
        public ActionResult RequestRevision(VMDOC_DocumentModel item)
        {
            var data = item.Load();
            return View(data);
        }

        [PageInfo("Revizyon Talebi Onaylama", SHRoles.DokumanYonetimRolu)]
        public ActionResult RequestRevisionUpdate(Guid id)
        {
            var item = new VMDOC_DocumentModel
            {
                revisionRequest = new DOC_DocumentRevisionRequest
                {
                    id = id
                }
            }.LoadRequest();
            return View(item);
        }

        [PageInfo("Revizyon Talebi Onaylama Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult RequestRevisionUpdate(VMDOC_DocumentModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var res = item.RequestRevisionUpdate(userStatus.user.id);
            return Json(new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success("") : feedback.Warning("")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Doküman Revizyon Talep Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult RequestRevision(VMDOC_DocumentModel item, bool isPost = true)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.RequestRevision(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Doküman Düzenleme Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Update(VMDOC_DocumentModel item, bool isPost = true)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message, false,
                Request.UrlReferrer.AbsolutePath == "/DOC/VWDOC_Document/Update"
                ? Url.Action("Index", "VWDOC_Document", new { area = "DOC" })
                : Url.Action("Detail", "VWDOC_Document", new { area = "DOC", id = item.id })
                ) : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Doküman Silme Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var res = new VMDOC_DocumentModel { id = id }.Delete();
            var result = new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success(res.message) : feedback.Warning(res.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Doküman Versiyon Güncelleme Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost]
        public JsonResult UpdateVersion(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var version = db.GetDOC_DocumentVersionById(id);
            if (version == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("Versiyon bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            var otherVersions = db.GetDOC_DocumentVersionByDocumentId(version.documentId.Value).Where(x => x.id != version.id).ToArray();

            var trans = db.BeginTransaction();

            version.isActive = true;

            foreach (var otherVersion in otherVersions)
            {
                otherVersion.isActive = false;
            }

            var document = db.GetDOC_DocumentById(version.documentId.Value);
            if(document == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("Doküman bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }
            document.versionNumber = version.versionNumber;

            var dbres = db.UpdateDOC_DocumentVersion(version, true, trans);
            dbres &= db.BulkUpdateDOC_DocumentVersion(otherVersions, true, trans);
            dbres &= db.UpdateDOC_Document(document, true, trans);

            if (!dbres.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = dbres.result,
                    FeedBack = feedBack.Warning("Versiyon aktif etme işlemi başarısız oldu. " + dbres.message)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = dbres.result,
                    FeedBack = feedBack.Success("Versiyon aktif etme işlemi başarılı", false, Url.Action("Detail", "VWDOC_Document", new { area = "DOC", id = version.documentId }))
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [PageInfo("Doküman Dosyası", SHRoles.DokumanYonetimRolu)]
        public ActionResult DetailForm(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var vs = db.GetDOC_DocumentVersionByDocumentIdAndActive(id);
            return View(vs?.id);
        }
    }
}
