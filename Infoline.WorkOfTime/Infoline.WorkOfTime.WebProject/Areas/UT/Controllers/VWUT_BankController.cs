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
    public class VWUT_BankController : Controller
	{
		[PageInfo("Banka Tanımları", SHRoles.OnMuhasebe)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Bankalar Methodu", SHRoles.OnMuhasebe)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_Bank(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWUT_BankCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Bankalar Veri Methodu", SHRoles.OnMuhasebe,SHRoles. HakEdisBayiPersoneli)]

		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_Bank(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Banka Ekleme Sayfası", SHRoles.OnMuhasebe)]
		public ActionResult Insert()
		{
		    var data = new VWUT_Bank 
			{ 
				id = Guid.NewGuid(),
				code = BusinessExtensions.B_GetIdCode()
			};
		    return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Banka Ekleme Sayfası", SHRoles.OnMuhasebe)]
		public JsonResult Insert(UT_Bank item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
			item.code = item.code != null ? item.code : BusinessExtensions.B_GetIdCode();
			var dbresult = db.InsertUT_Bank(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Banka Güncelleme Sayfası", SHRoles.OnMuhasebe)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_BankById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Banka Ekleme Sayfası", SHRoles.OnMuhasebe)]
		public JsonResult Update(UT_Bank item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateUT_Bank(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[PageInfo("Banka Silme İşlemi", SHRoles.OnMuhasebe)]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_Bank { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_Bank(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


	}
}
