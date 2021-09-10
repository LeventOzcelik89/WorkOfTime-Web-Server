using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductionOperationController : Controller
	{
		[PageInfo("Üretim Emri Operasyonları DataSource", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductionOperation(condition).ToDataSourceResult(request);
			data.Total = db.GetVWPRD_ProductionOperationCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Üretim Emri Operasyonları DataSourceDropDown", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductionOperation(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Üretim Emri Operasyon Detayı", SHRoles.Personel, SHRoles.UretimYonetici)]
		public ActionResult Detail(VMPRD_ProductionOperationModel request)
		{
			var model = request.Load();
			return View(model);
		}

		[AllowEveryone]
		[PageInfo("Üretim Emri Operasyon Ekleme", SHRoles.Personel, SHRoles.UretimYonetici)]
		public ActionResult Insert(VMPRD_ProductionOperationModel request)
		{
			var model = request.Load();

			return View(model);
		}

		[PageInfo("Üretim Emri Operasyon Ekleme Metodu", SHRoles.Personel, SHRoles.UretimYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMPRD_ProductionOperationModel item, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.Insert(userStatus.user.id);
			if (dbresult.result == true && (item.status == (int)EnumPRD_ProductionOperationStatus.FormYuklendi))
			{
				new FileUploadSave(Request).SaveAs();
			}
			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message, false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message),
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Üretim Emri Operasyon Düzenleme", SHRoles.Personel, SHRoles.UretimYonetici)]
		public ActionResult Update(VMPRD_ProductionOperationModel request)
		{
			var model = request.Load();
			return View(model);
		}

		//ExportPDF eklenir ise form render js çalışmıyor.
		[PageInfo("Üretim Emri Operasyon Formu Yazdır"), ExportPDF, AllowEveryone]
		public ActionResult Print(VMPRD_ProductionModel request, Guid? operationId)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var model = request.Load();
			model.productionOperations = model.productionOperations.Where(a => a.id == operationId).ToList();
			return View(model);
		}

		[PageInfo("Üretim Emri Operasyon Düzenleme Metodu", SHRoles.Personel, SHRoles.UretimYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMPRD_ProductionOperationModel item, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = item.Update(userStatus.user.id);
			if (dbresult.result == true && (item.status == (int)EnumFTM_TaskOperationStatus.GorevDosyaIslemiAltUrun || item.status == (int)EnumFTM_TaskOperationStatus.GorevDosyaIslemiAnaUrun || item.status == (int)EnumFTM_TaskOperationStatus.IslakImzaliFormYuklendi))
			{
				new FileUploadSave(Request).SaveAs();
			}

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Güncelleme işlemi başarısız")
			}, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[PageInfo("Stok&Envanter İşlem Sil", SHRoles.Personel, SHRoles.UretimYonetici)]
		public JsonResult Delete(Guid transactionId)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = new ResultStatus { result = true };

			dbresult &= new VMPRD_ProductionTransactionModel { id = transactionId }.Delete(userStatus.user.id);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning(dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
