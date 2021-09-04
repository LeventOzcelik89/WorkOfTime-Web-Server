using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
	public class VWSH_PublicationsController : Controller
	{
		[PageInfo("Yayın Bilgileri", SHRoles.IKYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Yayın Bilgileri Methodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_Publications(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PublicationsCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Yayın Bilgileri Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_Publications(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Yayın Bilgileri Detayı", SHRoles.IKYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PublicationsById(id);
		    return View(data);
		}

		[PageInfo("Yayın Bilgisi Ekleme", SHRoles.IKYonetici)]
		public ActionResult Insert()
		{
		    var data = new VWSH_Publications { id = Guid.NewGuid() };
		    return View(data);
		}

		[PageInfo("Yayın Bilgisi Ekleme", SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(SH_Publications item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_Publications(item);
			var res = new FileUploadSave(Request).SaveAs();
			var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Yayın Bilgisi Güncelleme", SHRoles.IKYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PublicationsById(id);
			return View(data);
		}

		[PageInfo("Yayın Bilgisi Güncelleme", SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SH_Publications item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_Publications(item,true);
			var res = new FileUploadSave(Request).SaveAs();
			var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Yayın Bilgisi Silme", SHRoles.IKYonetici)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_Publications { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_Publications(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
