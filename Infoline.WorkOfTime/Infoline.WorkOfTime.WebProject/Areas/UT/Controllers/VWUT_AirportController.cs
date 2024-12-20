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
	public class VWUT_AirportController : Controller
	{
		[PageInfo("Havaliman Listesi", SHRoles.Personel,SHRoles.SistemYonetici,SHRoles.IdariPersonel)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Havaliman Metodu", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_Airport(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWUT_AirportCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Havaliman Veri Metodu", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_Airport(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Havaliman Detayı", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_AirportById(id);
		    return View(data);
		}

		[PageInfo("Havaliman Ekleme", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		public ActionResult Insert()
		{
		    var data = new VWUT_Airport { id = Guid.NewGuid() };
		    return View(data);
		}

		[PageInfo("Havaliman Ekleme", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(UT_Airport item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertUT_Airport(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Havaliman Düzenle", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_AirportById(id);
		    return View(data);
		}

		[PageInfo("Havaliman Düzenle", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(UT_Airport item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateUT_Airport(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Havaliman Sil", SHRoles.Personel, SHRoles.SistemYonetici, SHRoles.IdariPersonel)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_Airport { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_Airport(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
