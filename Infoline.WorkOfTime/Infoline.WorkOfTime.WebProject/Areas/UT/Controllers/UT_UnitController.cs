using Infoline.Framework.Database;
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
    public class UT_UnitController : Controller
    {
        [PageInfo("Birim Tanımları", SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            ViewBag.Card = db.GetUT_UnitCard();
            return View();
		}


        [PageInfo("Birimler Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_Unit(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetUT_UnitCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Birimler Veri Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
            request.PageSize = int.MaxValue;
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_Unit(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Birim Ekleme", SHRoles.Personel)]
        public ActionResult Insert(UT_Unit item)
		{
		    return View(item);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Birim Ekleme", SHRoles.Personel)]
        public JsonResult Insert(UT_Unit item,bool? isPost)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;

			var nameControl = db.GetUT_UnitByName(item.name);
			if (nameControl != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı Birim Kaydı Zaten Sistemde Bulunmaktadır.")
				}, JsonRequestBehavior.AllowGet);
			}


			var dbresult = db.InsertUT_Unit(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Birim Düzenleme", SHRoles.SistemYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetUT_UnitById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Birim Düzenleme", SHRoles.SistemYonetici)]
        public JsonResult Update(UT_Unit item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
			var dbresult = new ResultStatus();

			if (db.GetUT_UnitByNameByUpdateName(item.id, item.name) == null)
			{
				item.changed = DateTime.Now;
				item.changedby = userStatus.user.id;

				dbresult = db.UpdateUT_Unit(item);
			}
			else
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı kayıt zaten mevcut. Lütfen farklı bir birim giriniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Birim Silme", SHRoles.SistemYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_Unit { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_Unit(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
