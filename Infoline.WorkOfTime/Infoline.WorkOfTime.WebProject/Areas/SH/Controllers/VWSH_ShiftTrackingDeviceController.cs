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

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
	public class VWSH_ShiftTrackingDeviceController : Controller
	{
		[PageInfo("PDKS Cihaz Listesi", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("PDKS Cihaz Listesi Veri Methodu", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_ShiftTrackingDevice(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_ShiftTrackingDeviceCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("PDKS Cihaz Listesi Veri Methodu", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_ShiftTrackingDevice(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("PDKS Cihaz Detayı", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_ShiftTrackingDeviceById(id);
		    return View(data);
		}

		[PageInfo("PDKS Cihaz Ekleme", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Insert()
		{
		    var data = new VWSH_ShiftTrackingDevice { id = Guid.NewGuid() };
			data.DeviceCode = data.DeviceCode ?? BusinessExtensions.B_GetIdCode();
			return View(data);
		}

		[PageInfo("PDKS Cihaz Ekleme", SHRoles.SistemYonetici, SHRoles.IKYonetici)]

		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(SH_ShiftTrackingDevice item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_ShiftTrackingDevice(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("PDKS Cihaz Güncelleme", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_ShiftTrackingDeviceById(id);
		    return View(data);
		}

		[PageInfo("PDKS Cihaz Güncelleme", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SH_ShiftTrackingDevice item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_ShiftTrackingDevice(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("PDKS Cihaz Silme", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_ShiftTrackingDevice { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_ShiftTrackingDevice(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
