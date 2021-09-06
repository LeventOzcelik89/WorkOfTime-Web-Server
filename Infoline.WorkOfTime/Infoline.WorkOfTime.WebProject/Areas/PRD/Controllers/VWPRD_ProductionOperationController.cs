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

		[PageInfo("Üretim Emri Operasyon Detayı", SHRoles.Personel)]
		public ActionResult Detail(VMPRD_ProductionOperationModel request)
		{
			var model = request.Load();
			return View(model);
		}

		[AllowEveryone]
		[PageInfo("Üretim Emri Operasyon Ekleme", SHRoles.Personel, SHRoles.YukleniciPersoneli)]
		public ActionResult Insert(VMPRD_ProductionOperationModel request)
		{
			var model = request.Load();

			return View(model);
		}

		[PageInfo("Üretim Emri Operasyon Ekleme Metodu", SHRoles.Personel)]
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

		[PageInfo("Saha Görev Operasyon Düzenleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
		public ActionResult Update(VMFTM_TaskOperationModel request)
		{
			var model = request.Load();
			return View(model);
		}

		//ExportPDF eklenir ise form render js çalışmıyor.
		[PageInfo("Görev Operasyon Formu Yazdır"), ExportPDF, AllowEveryone]
		public ActionResult Print(VMFTM_TaskModel request, Guid? operationId)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var model = request.Load();
			model.taskOperations = model.taskOperations.Where(a => a.id == operationId).ToList();
			return View(model);
		}
		[PageInfo("Saha Görev Operasyon Düzenleme Metodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMFTM_TaskOperationModel item, bool? isPost)
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

		[PageInfo("Saha Görev Operasyon Silme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
		[HttpPost]
		public JsonResult Delete(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			var operation = db.GetVWFTM_TaskOperationById(id);
			var dbresult = new ResultStatus { result = true };
			if (operation != null)
			{
				dbresult &= db.DeleteFTM_TaskOperation(new FTM_TaskOperation { id = id });
				if (operation.formResultId.HasValue)
				{
					dbresult &= db.DeleteFTM_TaskFormRelation(new FTM_TaskFormRelation { id = operation.formResultId.Value });
				}
			}
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Silme işlemi başarılı")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
