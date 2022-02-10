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

namespace Infoline.WorkOfTime.WebProject.Areas.UT.Controllers
{
	public class VWUT_TemplateController : Controller
	{
		[AllowEveryone]
		[PageInfo("Şablon Listesi")]
		public ActionResult Index()
		{
		    return View();
		}


		[AllowEveryone]
		[PageInfo("Şablon Listesi Veri Kaynağı")]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_Template(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWUT_TemplateCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Şablon Listesi Dropdown Methodu")]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_Template(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Şablon Detayı")]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_TemplateById(id);
		    return View(data);
		}


		[AllowEveryone]
		[PageInfo("Şablon Ekleme")]
		public ActionResult Insert()
		{
		    var data = new VWUT_Template { id = Guid.NewGuid() };
		    return View(data);
		}


		[AllowEveryone]
		[PageInfo("Şablon Ekleme ")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(UT_Template item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertUT_Template(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		[PageInfo("Şablon Güncelleme")]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_TemplateById(id);
		    return View(data);
		}


		[AllowEveryone]
		[PageInfo("Şablon Güncelleme")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(UT_Template item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateUT_Template(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		[PageInfo("Şablon Silme")]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_Template { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_Template(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
