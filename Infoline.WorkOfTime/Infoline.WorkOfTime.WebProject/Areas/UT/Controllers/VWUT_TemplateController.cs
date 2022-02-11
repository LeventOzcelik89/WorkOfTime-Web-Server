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
		    return View(new VMUT_TemplateModel { id = id}.Load());
		}


		[AllowEveryone]
		[PageInfo("Şablon Ekleme")]
		public ActionResult Insert(VMUT_TemplateModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[AllowEveryone]
		[PageInfo("Şablon Ekleme ")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMUT_TemplateModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success("Şablon kaydetme işlemi başarılı", false, Url.Action("Index", "VWUT_Template")) :
						   feedback.Warning("Şablon kaydetme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		[PageInfo("Şablon Güncelleme")]
		public ActionResult Update(VMUT_TemplateModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[AllowEveryone]
		[PageInfo("Şablon Güncelleme")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMUT_TemplateModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success("Şablon güncelleme işlemi başarılı", false, Url.Action("Index", "VWUT_Template")) :
						   feedback.Warning("Şablon güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		[PageInfo("Şablon Silme")]
		[HttpPost]
		public JsonResult Delete(Guid id)
		{
			return Json(new ResultStatusUI(new VMUT_TemplateModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);

		}



	}
}
