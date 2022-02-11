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
	public class VWSH_CorrectiveActivityController : Controller
	{
		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Listesi")]
		public ActionResult Index()
		{
		    return View();
		}


		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Veri Methodu")]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_CorrectiveActivity(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_CorrectiveActivityCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Dropdown Methodu")]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_CorrectiveActivity(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Detayı")]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSH_CorrectiveActivityModel { id = id }.Load());
		}


		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Ekleme")]
		public ActionResult Insert(VMSH_CorrectiveActivityModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Ekleme")]
		public JsonResult Insert(VMSH_CorrectiveActivityModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Şablon kaydetme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);

		}


		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Güncelleme")]
		public ActionResult Update(VMSH_CorrectiveActivityModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Güncelleme")]
		public JsonResult Update(VMSH_CorrectiveActivityModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Şablon güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[AllowEveryone]
		[PageInfo("Düzenleyici Önleyici Faaliyet Silme")]
		public JsonResult Delete(Guid id)
		{
			return Json(new ResultStatusUI(new VMSH_CorrectiveActivityModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
		}


	}
}
