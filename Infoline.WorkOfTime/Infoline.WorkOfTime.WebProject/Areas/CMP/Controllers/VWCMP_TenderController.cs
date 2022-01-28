using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_TenderController : Controller
	{
		[PageInfo("Satış Teklifleri", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis, SHRoles.CRMBayiPersoneli, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult IndexSelling()
		{
			return View();
		}

		[PageInfo("Satın Alma Teklifleri", SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaOnaylayici, SHRoles.MuhasebeAlis)]
		public ActionResult IndexBuying()
		{
			return View();
		}

		[PageInfo("Teklifler Metodu", SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis, SHRoles.SatisOnaylayici, SHRoles.SatisFatura, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli,SHRoles.SatinAlmaOnaylayiciGorev)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var tenders = db.GetVWCMP_Tender(condition);

			foreach (var item in tenders)
			{
				item.description = null;
				item.tenderConditions = null;
			}

			var data = tenders.RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWCMP_TenderCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Teklifler Adet Metodu", SHRoles.Personel, SHRoles.CRMBayiPersoneli)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var total = db.GetVWCMP_TenderCount(condition.Filter);
			return total;
		}

		[PageInfo("Teklifler Veri Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var tenders = db.GetVWCMP_Tender(condition);
			foreach (var item in tenders)
			{
				item.description = null;
				item.tenderConditions = null;
			}

			return Content(Infoline.Helper.Json.Serialize(tenders), "application/json");
		}

		[PageInfo("Alış Teklifi Detayı", SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.MuhasebeAlis)]
		public ActionResult DetailBuying(Guid id)
		{
			var data = new VMCMP_TenderModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Alis);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[PageInfo("Satış Teklifi Detayı", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.MuhasebeSatis, SHRoles.CRMBayiPersoneli, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult DetailSelling(Guid id)
		{
			var data = new VMCMP_TenderModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Satis);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[PageInfo("Satış Teklifi Detayı (Müşteri)")]
		public ActionResult DetailSellingCustomer(Guid id)
		{
			var data = new VMCMP_TenderModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Satis);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[PageInfo("Alış Teklifi Ekleme", SHRoles.SatinAlmaPersonel)]
		public ActionResult InsertBuying(VMCMP_TenderModels item, bool? transform = false)
		{
			item.Load(transform, (int)EnumCMP_InvoiceDirectionType.Alis);
			return View(item);
		}

		[PageInfo("Satış Teklifi Ekleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult InsertSelling(VMCMP_TenderModels item)
		{
			item.Load(false, (int)EnumCMP_InvoiceDirectionType.Satis);
			var userStatus = (PageSecurity)Session["userStatus"];

			if (item.presentationId.HasValue)
			{
				item.supplierId = userStatus.user.CompanyId.HasValue ? userStatus.user.CompanyId.Value : item.supplierId;
			}

			if (item.taskId.HasValue)
			{
				item.supplierId = userStatus.user.CompanyId;
			}

			return View(item);
		}

		[HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
		[PageInfo("Teklif Ekleme Metodu", SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.SatinAlmaOnaylayiciGorev)]
		public JsonResult Insert(VMCMP_TenderModels item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = item.Save(userStatus.user.id, Request);

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Teklif kaydetme işlemi başarılı", false, Request.UrlReferrer.AbsoluteUri.Contains("/CRM/VWCRM_Presentation/Detail?") ? Request.UrlReferrer.AbsoluteUri : null) :
						   feedback.Warning("Teklif kaydetme işlemi başarısız. Mesaj : " + dbresult.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Satın Alma Teklifi Düzenleme", SHRoles.SatinAlmaPersonel,SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult UpdateBuying(Guid id)
		{
			var data = new VMCMP_TenderModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Alis);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[PageInfo("Satış Teklifi Düzenleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult UpdateSelling(Guid id)
		{
			var data = new VMCMP_TenderModels { id = id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Satis);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
		[PageInfo("Teklif Düzenleme Metodu", SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.SatinAlmaOnaylayiciGorev)]
		public JsonResult Update(VMCMP_TenderModels item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = item.Save(userStatus.user.id, Request);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Satın Alma talebi düzenleme işlemi başarılı", false) :
						   feedback.Warning("Satın Alma talebi düzenleme işlemi başarısız. Mesaj : " + dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Teklif Koşul Dosyası Ekleme", SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.SatinAlmaOnaylayiciGorev)]
		public JsonResult EditorUploadFile(HttpPostedFileBase upload, string id)
		{
			if (upload != null)
			{
				var rs = new LocalFileProvider("CMP_Invoice").Import(new Guid(id), "Teklif Koşul Dosyası", upload);

				var returnObj = new
				{
					uploaded = 1,
					fileName = ((Dictionary<string, object>)rs.Object)["name"],
					url = ((Dictionary<string, object>)rs.Object)["url"],
				};
				return Json(returnObj, JsonRequestBehavior.AllowGet);
			}
			return Json(new { uploaded = 0, error = new { message = "İşlem Sırasında Hata Meydana Geldi" } }, JsonRequestBehavior.AllowGet);

		}

		[PageInfo("Teklif Not Ekleme Metodu", SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisOnaylayici, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ContentResult InsertNote(Guid tenderId, string note)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbres = new VMCMP_TenderModels { id = tenderId }.Load(false, null).InsertNote(userStatus.user.id, note);
			return Content(Infoline.Helper.Json.Serialize(dbres), "application/json");
		}

		[PageInfo("Teklif Onay-Red Metodu", SHRoles.SatinAlmaOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisOnaylayici,SHRoles.SatinAlmaOnaylayiciGorev)]
		public ContentResult UpdateStatus(Guid tenderId, int type, bool isTaskRule)
		{
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];

			var dbresult = new VMCMP_TenderModels { id = tenderId }.Load(false, null).UpdateStatus(type, userStatus.user.id, isTaskRule);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = dbresult.objects,
				FeedBack = dbresult.result ? feedback.Success("İşlem başarılı Mesaj : " + dbresult.message, false) :
						   feedback.Warning("İşlem başarısız. Mesaj : " + dbresult.message)
			};

			return Content(Infoline.Helper.Json.Serialize(result), "application/json");
		}

		[PageInfo("Teklif Karşılaştırma", SHRoles.SatinAlmaOnaylayici)]
		public ActionResult TenderCompare(Guid requestId)
		{
			var db = new WorkOfTimeDatabase();
			var transformTenders = db.GetCMP_InvoiceTransformByIsTransformedFrom(requestId);
			var tenders = db.GetVWCMP_TenderByIds(transformTenders.Where(a => a.invoiceIdTo != null).Select(a => a.invoiceIdTo.Value).ToArray());
			var list = new List<VMCMP_TenderModels>();
			list.AddRange(tenders.Select(a => new VMCMP_TenderModels { id = a.id }.Load(false, null)).ToList());
			return View(list.ToArray());
		}

		[HttpPost]
		[PageInfo("Teklif Silme Metodu", SHRoles.SatisPersoneli, SHRoles.SatinAlmaPersonel)]
		public JsonResult Delete(Guid id)
		{
			var feedback = new FeedBack();
			var dbresult = new VMCMP_TenderModels { id = id }.Delete();

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Satış Teklifi Yazdır"), ExportPDF, AllowEveryone]
		public ActionResult Print(Guid id, int? type)
		{
			var model = new VMCMP_TenderModels { id = id }.Load(false, null).GetFormTemplate(type);
			return View(model);
		}
	}
}
