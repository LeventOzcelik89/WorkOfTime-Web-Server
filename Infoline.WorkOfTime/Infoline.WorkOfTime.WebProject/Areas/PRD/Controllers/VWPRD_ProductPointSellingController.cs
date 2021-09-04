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
    public class VWPRD_ProductPointSellingController : Controller
	{
        [PageInfo("Ürün Puanları", SHRoles.CRMYonetici)]
        public ActionResult Index()
		{
            return View();
        }

        [PageInfo("Ürün Satış Puanları Grid Metodu",SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPointSelling(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_ProductPointSellingCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Ürün Satış Puanları Dropdown Grid Metodu",SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPointSelling(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Ürün Satış Puanları Ekleme",  SHRoles.CRMYonetici)]
        public ActionResult Insert(VWPRD_ProductPointSelling data)
		{
		    return View(data);
		}

        [PageInfo("Ürün Satış Puanları Ekleme",  SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(PRD_ProductPointSelling item)
		{
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
            var dbresult = new VMPRD_ProductModel { id = item.productId.Value }.UpsertPointSelling(item);
		    return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Ürün Satış Puanları Düzenle",  SHRoles.CRMYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPointSellingById(id);
		    return View(data);
		}

        [PageInfo("Ürün Satış Puanları Düzenle",  SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(PRD_ProductPointSelling item)
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
            var dbresult = new VMPRD_ProductModel { id = item.productId.Value }.UpsertPointSelling(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Ürün Satış Puanları Silme",  SHRoles.CRMYonetici)]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new PRD_ProductPointSelling { id = new Guid(a) });
		
		    var dbresult = db.BulkDeletePRD_ProductPointSelling(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
