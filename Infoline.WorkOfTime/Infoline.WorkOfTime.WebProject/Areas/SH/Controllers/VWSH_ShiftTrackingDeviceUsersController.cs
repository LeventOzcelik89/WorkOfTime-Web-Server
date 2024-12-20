﻿using Infoline.WorkOfTime.BusinessData;
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
	public class VWSH_ShiftTrackingDeviceUsersController : Controller
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
		    var data = db.GetVWSH_ShiftTrackingDeviceUsers(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_ShiftTrackingDeviceUsersCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_ShiftTrackingDeviceUsers(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_ShiftTrackingDeviceUsersById(id);
		    return View(data);
		}


		[AllowEveryone]
		public ActionResult Insert()
		{
		    var data = new VWSH_ShiftTrackingDeviceUsers { id = Guid.NewGuid() };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		public JsonResult Insert(SH_ShiftTrackingDeviceUsers item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_ShiftTrackingDeviceUsers(item);
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
		    var data = db.GetVWSH_ShiftTrackingDeviceUsersById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		public JsonResult Update(SH_ShiftTrackingDeviceUsers item)
		{
		    var db = new WorkOfTimeDatabase();
			var trans = db.BeginTransaction();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
			var shifts = db.GetSH_ShiftTrackingByDeviceIdAndUserDeviceId(item.deviceId.Value, item.deviceUserId).ToList();
			shifts.ForEach(s => s.userId = item.userId);
			

			var dbresult = db.UpdateSH_ShiftTrackingDeviceUsers(item, false, trans);
			dbresult = db.BulkUpdateSH_ShiftTracking(shifts, false, trans);

            if (dbresult.result)
            {
				trans.Commit();
				var result = new ResultStatusUI
				{
					Result = dbresult.result,
					FeedBack = feedback.Success("Güncelleme işlemi başarılı")
				};

				return Json(result, JsonRequestBehavior.AllowGet);
			}
            else
            {
				trans.Rollback();
				var result = new ResultStatusUI
				{
					Result = dbresult.result,
					FeedBack = feedback.Error("Güncelleme işlemi başarısız", dbresult.message)
				};

				return Json(result, JsonRequestBehavior.AllowGet);
			}
		   
		}


		[HttpPost]
		[AllowEveryone]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_ShiftTrackingDeviceUsers { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_ShiftTrackingDeviceUsers(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
