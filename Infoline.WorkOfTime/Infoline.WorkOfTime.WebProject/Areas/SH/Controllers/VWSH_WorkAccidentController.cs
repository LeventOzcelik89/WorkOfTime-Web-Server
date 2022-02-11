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
	public class VWSH_WorkAccidentController : Controller
	{
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Listesi")]
		public ActionResult Index()
		{
		    return View();
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Veri Methodu")]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccident(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_WorkAccidentCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Dropdown Methodu")]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccident(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Detayı")]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Ekleme")]
		public ActionResult Insert(VMSH_WorkAccidentModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Ekleme")]
		public JsonResult Insert(VMSH_WorkAccidentModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay bildirim kaydetme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Güncelleme")]
		public ActionResult Update(VMSH_WorkAccidentModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Güncelleme")]
		public JsonResult Update(VMSH_WorkAccidentModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay bildirim güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Silme")]
		public JsonResult Delete(Guid id)
		{
			return Json(new ResultStatusUI(new VMSH_WorkAccidentModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
		}

	}
}
