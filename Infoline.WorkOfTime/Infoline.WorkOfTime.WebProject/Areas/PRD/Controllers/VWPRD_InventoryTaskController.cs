using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_InventoryTaskController : Controller
    {
        [PageInfo("Bakım Periyotları", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Envanter Bakım Görev Listesi DataSource", SHRoles.Personel, SHRoles.SahaGorevMusteri)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_InventoryTask(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_InventoryTaskCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Envanter Bakım Görev Listesi DataSourceDropDown", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_InventoryTask(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Envanter Bakım Miktar DataSource", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWPRD_InventoryTaskCount(condition.Filter);
            return count;
        }

        [PageInfo("Envanter Bakım Görev Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Insert(VWPRD_InventoryTask item)
        {
            var data = new VWPRD_InventoryTask { id = Guid.NewGuid() };
            if (item.inventoryId.HasValue)
            {
                data.inventoryId = item.inventoryId.Value;
            }

            return View(data);
        }

        [PageInfo("Envanter Bakım Görev Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(PRD_InventoryTask item, Guid[] inventoryIds)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var trans = db.BeginTransaction();
            var dbResult = new ResultStatus { result = true };
            foreach (var id in inventoryIds)
            {
                item.inventoryId = id;
                item.id = Guid.NewGuid();
                dbResult &= db.InsertPRD_InventoryTask(item, trans);
            }

            if (dbResult.result == true) { trans.Commit(); } else { trans.Rollback(); }


            var result = new ResultStatusUI
            {
                Result = dbResult.result,
                FeedBack = dbResult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Envanter Bakım Görev Güncelleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_InventoryTaskById(id);
            return View(data);
        }

        [PageInfo("Envanter Bakım Görev Güncelleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(PRD_InventoryTask item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdatePRD_InventoryTask(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Envanter Bakım Görev Silme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new PRD_InventoryTask { id = new Guid(a) });

            var dbresult = db.BulkDeletePRD_InventoryTask(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
