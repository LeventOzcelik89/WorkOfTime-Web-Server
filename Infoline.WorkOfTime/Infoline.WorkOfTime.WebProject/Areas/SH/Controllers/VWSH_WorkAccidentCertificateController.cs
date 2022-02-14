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
	public class VWSH_WorkAccidentCertificateController : Controller
	{
		[PageInfo("Kaza Ve Olay Eğitim  Listesi", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
		public ActionResult Index()
		{
		    return View();
		}


		[PageInfo("Kaza Ve Olay Eğitim Veri Methodu", SHRoles.Personel, SHRoles.SistemYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccidentCertificate(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_WorkAccidentCertificateCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Kaza Ve Olay Eğitim Dropdown Methodu", SHRoles.Personel, SHRoles.SistemYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccidentCertificate(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Kaza Ve Olay Eğitim Detayı", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Eğitim Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.IKYonetici)]
		public ActionResult Insert(VMSH_WorkAccidentCertificateModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Kaza Ve Olay Eğitim Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.IKYonetici)]
		public JsonResult Insert(VMSH_WorkAccidentCertificateModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay eğitimi kaydetme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Kaza Ve Olay Eğitim Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.IKYonetici)]
		public ActionResult Update(VMSH_WorkAccidentCertificateModel model)
		{
			var data = model.Load();
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Kaza Ve Olay Eğitim Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.IKYonetici)]
		public JsonResult Update(VMSH_WorkAccidentCertificateModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay eğitimi güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[PageInfo("Kaza Ve Olay Eğitim Silme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.IKYonetici)]
		public JsonResult Delete(Guid id)
		{
			return Json(new ResultStatusUI(new VMSH_WorkAccidentCertificateModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);

		}



	}
}
