using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.UT.Controllers
{
    public class UT_CurrencyController : Controller
    {
        [PageInfo("Para Birimleri",SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            ViewBag.Card = db.GetUT_CurrencyCard();
            return View();
        }


        [PageInfo("Para Birimleri Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_Currency(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetUT_CurrencyCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Para Birimleri Veri Metodu", SHRoles.Personel,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
            request.PageSize = int.MaxValue;
            var condition = KendoToExpression.Convert(request);
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_Currency(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Para Birimi Detayı", SHRoles.SistemYonetici)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_CurrencyById(id);
		    return View(data);
		}


        [PageInfo("Para Birimi Ekleme", SHRoles.Personel)]
        public ActionResult Insert()
		{
		    var data = new UT_Currency { id = Guid.NewGuid() };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Para Birimi Ekleme", SHRoles.Personel)]
        public JsonResult Insert(UT_Currency item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertUT_Currency(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Para Birimi Düzenleme", SHRoles.SistemYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_CurrencyById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Para Birimi Düzenleme", SHRoles.SistemYonetici)]
        public JsonResult Update(UT_Currency item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateUT_Currency(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Para Birimi Silme", SHRoles.SistemYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_Currency { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_Currency(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
