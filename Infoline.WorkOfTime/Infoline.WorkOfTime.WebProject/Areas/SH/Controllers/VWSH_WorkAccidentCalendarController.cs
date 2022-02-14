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
	public class VWSH_WorkAccidentCalendarController : Controller
	{
		[PageInfo("Kaza Ve Olay Etkinlik Listesi", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici)]
		public ActionResult Index()
		{
		    return View();
		}



		[PageInfo("Kaza Ve Olay Etkinlik Veri Methodu", SHRoles.Personel, SHRoles.SistemYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccidentCalendar(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_WorkAccidentCalendarCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}



		[PageInfo("Kazas Ve Olay Etkinlik Dropdown Methodu", SHRoles.Personel, SHRoles.SistemYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccidentCalendar(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Kazas Ve Olay Etkinlik Detayı", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici)]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}



		[PageInfo("Kazas Ve Olay Etkinlik Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public ActionResult Insert(VMSH_WorkAccidentCalendarModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]

		[PageInfo("Kazas Ve Olay Etkinlik Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public JsonResult Insert(VMSH_WorkAccidentCalendarModel model, bool? isPost)
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



		[PageInfo("Kazas Ve Olay Etkinlik Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public ActionResult Update(VMSH_WorkAccidentCalendarModel model)
		{
			var data = model.Load();
			return View(data);
		}



		[PageInfo("Kazas Ve Olay Etkinlik Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMSH_WorkAccidentCalendarModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay etkinliği güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]

		[PageInfo("Kazas Ve Olay Etkinlik Silme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public JsonResult Delete(Guid id)
		{
			return Json(new ResultStatusUI(new VMSH_WorkAccidentCalendarModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);

		}

	}
}
