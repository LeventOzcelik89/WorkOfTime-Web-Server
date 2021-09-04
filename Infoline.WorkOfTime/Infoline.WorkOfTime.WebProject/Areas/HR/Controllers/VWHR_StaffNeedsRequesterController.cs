using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HR.Controllers
{
	public class VWHR_StaffNeedsRequesterController : Controller
	{
        [PageInfo("Personel Talepleri", SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Personel Talepleri Veri ", SHRoles.PersonelTalebi, SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsRequester(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWHR_StaffNeedsRequesterCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Personel Talepleri Dropdown", SHRoles.PersonelTalebi, SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsRequester(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Personel Talepleri Adet Metodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            return db.GetVWHR_StaffNeedsRequesterCount(condition.Filter);
        }

        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsRequesterById(id);
		    return View(data);
		}

		public ActionResult Insert()
		{
		    var data = new VWHR_StaffNeedsRequester { id = Guid.NewGuid() };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_StaffNeedsRequester item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_StaffNeedsRequester(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsRequesterById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_StaffNeedsRequester item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_StaffNeedsRequester(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new HR_StaffNeedsRequester { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteHR_StaffNeedsRequester(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
