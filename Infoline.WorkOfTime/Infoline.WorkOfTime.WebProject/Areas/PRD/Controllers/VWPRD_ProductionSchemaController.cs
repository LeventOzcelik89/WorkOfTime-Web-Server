using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductionSchemaController : Controller
	{
		[PageInfo("Üretim Şeması Listesi", SHRoles.Personel)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Üretim Şeması Grid Metodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductionSchema(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_ProductionSchemaCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Üretim Şeması Dropdown Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductionSchema(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Üretim Şeması Detayı", SHRoles.Personel)]
		public ActionResult Detail(VWPRD_ProductionSchemaModel model)
		{
		    return View(model.Load());
		}

		[PageInfo("Üretim Şeması Ekleme", SHRoles.Personel)]
		public ActionResult Insert(Guid? productId, VWPRD_ProductionSchemaModel model)
		{
			model.id = Guid.NewGuid();
			model.productId = productId;
			return View(model.Load());
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Üretim Şeması Ekleme", SHRoles.Personel)]
		public JsonResult Insert(VWPRD_ProductionSchemaModel item)
		{
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
			var dbresult = item.Save(userStatus.user.id, Request);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error(dbresult.message, "Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Üretim Şeması Güncelleme", SHRoles.StokYoneticisi, SHRoles.UretimYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductionSchemaById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Üretim Şeması Güncelleme", SHRoles.StokYoneticisi, SHRoles.UretimYonetici)]
		public JsonResult Update(VWPRD_ProductionSchemaModel item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.Save(userStatus.user.id, Request);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error(dbresult.message,"Güncelleme işlemi başarısız"),
			};
			result.FeedBack.message = dbresult.message;

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[PageInfo("Üretim Şeması Silme", SHRoles.StokYoneticisi, SHRoles.UretimYonetici)]
		public JsonResult Delete(Guid id)
		{
			var feedback = new FeedBack();
			var dbresult = new VWPRD_ProductionSchemaModel { id = id }.Delete();
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error(dbresult.message,"Silme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);			
		}

	}
}
