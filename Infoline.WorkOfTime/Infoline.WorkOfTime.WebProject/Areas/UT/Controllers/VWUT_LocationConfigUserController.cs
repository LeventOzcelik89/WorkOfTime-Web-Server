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

namespace Infoline.WorkOfTime.WebProject.Areas.UT.Controllers
{
	public class VWUT_LocationConfigUserController : Controller
	{
		[PageInfo("Lokasyon Takip Ekleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Lokasyon Takip Ekleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_LocationConfigUser(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWUT_LocationConfigUserCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Lokasyon Takip Ekleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_LocationConfigUser(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Lokasyon Takip Detay Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_LocationConfigUserById(id);
		    return View(data);
		}

		[PageInfo("Lokasyon Takip Ekleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public ActionResult Insert()
		{
		    var data = new VWUT_LocationConfigUser { id = Guid.NewGuid() };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Lokasyon Takip Ekleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public JsonResult Insert(UT_LocationConfigUser item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertUT_LocationConfigUser(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Lokasyon Takip Güncelleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_LocationConfigUserById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Lokasyon Takip Güncelleme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public JsonResult Update(UT_LocationConfigUser item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateUT_LocationConfigUser(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[PageInfo("Lokasyon Takip Silme Konfigürasyon Sayfası", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici)]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_LocationConfigUser { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_LocationConfigUser(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}