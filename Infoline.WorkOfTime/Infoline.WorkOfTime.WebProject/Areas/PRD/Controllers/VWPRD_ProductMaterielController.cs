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
    public class VWPRD_ProductMaterielController : Controller
    {
        [PageInfo("Ürün Ağacı Tanımları", SHRoles.StokYoneticisi)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Ürün Ağacı DataSource", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductMateriel(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductMaterielCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Ağacı DataSourceDropDown", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductMateriel(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Ağacı Ekleme", SHRoles.StokYoneticisi)]
        public ActionResult Insert(VWPRD_ProductMateriel data)
        {
            return View(data);
        }

        [PageInfo("Ürün Ağacı Ekleme", SHRoles.StokYoneticisi)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(PRD_ProductMateriel item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.createdby = userStatus.user.id;

            if (item.productId == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Ürün seçimi zorunludur.")
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = new VMPRD_ProductModel { id = item.productId.Value }.UpsertProductMateriel(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Ürün Ağacı Güncelleme", SHRoles.StokYoneticisi)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductMaterielById(id);
            return View(data);
        }

        [PageInfo("Ürün Ağacı Güncelleme", SHRoles.StokYoneticisi)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(PRD_ProductMateriel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changedby = userStatus.user.id;
            
            if (item.productId == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Ürün seçimi zorunludur.")
                }, JsonRequestBehavior.AllowGet);
            }
            var dbresult = new VMPRD_ProductModel { id = item.productId.Value }.UpsertProductMateriel(item);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Ürün Ağacı Silme", SHRoles.StokYoneticisi)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new PRD_ProductMateriel { id = new Guid(a) });

            var dbresult = db.BulkDeletePRD_ProductMateriel(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
