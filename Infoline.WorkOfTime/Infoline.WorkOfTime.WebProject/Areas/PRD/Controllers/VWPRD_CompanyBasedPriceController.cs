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
	public class VWPRD_CompanyBasedPriceController : Controller
	{
		[AllowEveryone]
		public ActionResult Index()
		{
		    return View();
		}

		[AllowEveryone]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_CompanyBasedPrice(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_CompanyBasedPriceCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_CompanyBasedPrice(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_CompanyBasedPriceById(id);
		    return View(data);
		}


		[AllowEveryone]
		public ActionResult Insert()
		{
		    var data = new VWPRD_CompanyBasedPrice { id = Guid.NewGuid() };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		public JsonResult Insert(PRD_CompanyBasedPrice item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertPRD_CompanyBasedPrice(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_CompanyBasedPriceById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		public JsonResult Update(PRD_CompanyBasedPrice item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdatePRD_CompanyBasedPrice(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[AllowEveryone]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new PRD_CompanyBasedPrice { id = new Guid(a) });
		
		    var dbresult = db.BulkDeletePRD_CompanyBasedPrice(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
