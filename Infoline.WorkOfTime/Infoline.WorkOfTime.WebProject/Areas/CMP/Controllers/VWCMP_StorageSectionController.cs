using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_StorageSectionController : Controller
    {
        [PageInfo("İşletme Depo Alt Birimleri")]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("İşletme Depo Alt Birimleri Verileri")]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_StorageSection(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_StorageSectionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("İşletme Depo Alt Birimleri Dropdown Verileri")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_StorageSection(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("İşletme Depo Alt Birimleri Detayı")]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_StorageSectionById(id);
            return View(data);
        }

        [PageInfo("İşletme Depo Alt Birimleri Ekleme")]
        public ActionResult Insert(VWCMP_StorageSection data)
        {
            var db = new WorkOfTimeDatabase();

            if (data.storageId.HasValue)
            {
                var storage = db.GetCMP_StorageById(data.storageId.Value);
                if (storage != null)
                {
                    data.companyId = storage.companyId;
                    data.storageId = storage.id;
                }

            }else
            {
                var storageSection = db.GetCMP_StorageSectionById(data.id);
                if (storageSection != null)
                {
                    data.pid = storageSection.id;
                    data.storageId = storageSection.storageId;
                    data.companyId = storageSection.companyId;
                }
            }

            if (string.IsNullOrEmpty(data.code))
            {
                data.code = BusinessExtensions.B_GetIdCode();
            }
            return View(data);
        }

        [PageInfo("İşletme Depo Alt Birimleri Ekleme İşlemi")]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CMP_StorageSection item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.id = Guid.NewGuid();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var dbresult = db.InsertCMP_StorageSection(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İşletme Depo Alt Birimleri Güncelleme")]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_StorageSectionById(id);
            return View(data);
        }

        [PageInfo("İşletme Depo Alt Birimleri Güncelleme İşlemi")]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CMP_StorageSection item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateCMP_StorageSection(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İşletme Depo Alt Birimleri Silme İşlemi")]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new CMP_StorageSection { id = new Guid(a) });

            var dbresult = db.BulkDeleteCMP_StorageSection(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Depo alt birimi silme işlemi başarılı") : feedback.Warning("Silmek istediğiniz birime ait alt birimler bulunduğundan işlemi gerçekleştiremezsiniz.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [PageInfo("Alt birimlerin gösterildiği sayfadır.",SHRoles.DepoSorumlusu)]
        public ActionResult TreeViewStorage(Guid id, bool LayoutPage = true)
        {
            var db = new WorkOfTimeDatabase();
            var cmpStorage = db.GetCMP_StorageById(id);
            ViewBag.LayoutPage = LayoutPage;
            return View(cmpStorage);
        }

        [PageInfo("Alt birimlerin yüklendiği methoddur.", SHRoles.DepoSorumlusu)]
        public JsonResult GetDataSourceTreeViewByPidId(Guid? SectionId, Guid storageId)
        {
            var db = new WorkOfTimeDatabase();
            var liste = new List<object>();
            var storagesSection = db.GetCMP_StorageSectionByPidId(SectionId, storageId).OrderBy(c => c.name).ToArray();
            foreach (var section in storagesSection)
            {
                var storageSection = db.GetCMP_StorageSectionByPidIdCount(section.id, storageId);
                var newStorageTreeView = new 
                {
                    SectionId = section.id,
                    FullName = section.name + " (" + section.code + ")",
                    HasChilds = storageSection > 0 ? true : false,
                };
                liste.Add(newStorageTreeView);
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
    }
}
