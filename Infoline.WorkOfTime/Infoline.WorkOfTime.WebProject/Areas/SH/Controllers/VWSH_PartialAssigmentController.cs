using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
	public class VWSH_PartialAssigmentController : Controller
	{
		[PageInfo("Kısmi Zamanlı Görevlendirme", SHRoles.IKYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PartialAssigment(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PartialAssigmentCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PartialAssigment(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Detay Methodu", SHRoles.IKYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PartialAssigmentById(id);
		    return View(data);
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Ekleme Methodu", SHRoles.IKYonetici)]
		public ActionResult Insert()
		{
		    var data = new VWSH_PartialAssigment { id = Guid.NewGuid() };
		    return View(data);
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Ekleme Methodu", SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(SH_PartialAssigment item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_PartialAssigment(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Güncelleme Methodu", SHRoles.IKYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PartialAssigmentById(id);
		    return View(data);
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Güncelleme Methodu", SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SH_PartialAssigment item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;

		    var dbresult = db.UpdateSH_PartialAssigment(item,true);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Kısmi Zamanlı Görevlendirme Silme Methodu", SHRoles.IKYonetici)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PartialAssigment { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PartialAssigment(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
		[PageInfo("Akademisyen Adlarının Çekildiği Metot", SHRoles.IKYonetici)]
		public JsonResult GetStaffNameList()
		{
			var db = new WorkOfTimeDatabase();
			var result = db.GetSH_PartialAssigmentStaffNameList();
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Bölüm Adlarının Çekildiği Metot", SHRoles.IKYonetici)]
		public JsonResult GetSchoolDepartmentList()
		{
			var db = new WorkOfTimeDatabase();
			var result = db.GetSH_PartialAssigmentSchoolDepartmentList();
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
