using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
	public class VWFTM_TaskOperationController : Controller
	{
		[PageInfo("Saha Görev Operasyonları DataSource", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWFTM_TaskOperation(condition).ToDataSourceResult(request);
			data.Total = db.GetVWFTM_TaskOperationCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Saha Görev Operasyonları DataSourceDropDown", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWFTM_TaskOperation(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Saha Görev Operasyon Detayı", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevMusteri)]
		public ActionResult Detail(VMFTM_TaskOperationModel request)
		{
			var model = request.Load();
			return View(model);
		}

		[PageInfo("Saha Görev Operasyon Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli)]
		public ActionResult Insert(VMFTM_TaskOperationModel request)
		{
			var model = request.Load();

			return View(model);
		}

		[PageInfo("Saha Görev Operasyon Ekleme Metodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMFTM_TaskOperationModel item, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.Insert(userStatus.user.id);
			if (dbresult.result == true && (item.status == (int)EnumFTM_TaskOperationStatus.GorevDosyaIslemiAltUrun || item.status == (int)EnumFTM_TaskOperationStatus.GorevDosyaIslemiAnaUrun || item.status == (int)EnumFTM_TaskOperationStatus.IslakImzaliFormYuklendi))
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
