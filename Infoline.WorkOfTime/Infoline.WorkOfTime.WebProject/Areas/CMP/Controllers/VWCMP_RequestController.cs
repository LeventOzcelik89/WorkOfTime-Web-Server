using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_RequestController : Controller
	{
		[PageInfo("Satın Alma Talepleri", SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi,SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult Index()
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var authorizedRoles = userStatus.AuthorizedRoles;
			var satinAlmaOnaylayiciGorev = new Guid(SHRoles.SatinAlmaOnaylayiciGorev);

			if (authorizedRoles.Contains(satinAlmaOnaylayiciGorev))
			{
				return RedirectToAction("IndexTask");
			}

			return View();
		}

		[PageInfo("Satın Alma Talepleri (Görev)", SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult IndexTask()
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var data = new VMCMP_RequestModels().Load(userStatus.user.id);

			return View(data);
		}

		[PageInfo("Satın Alma Talepleri Metodu", SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.ProjeYonetici, SHRoles.SahaGorevPersonel,SHRoles.SatinAlmaOnaylayiciGorev)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_Request(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWCMP_RequestCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Talepler Adet Metodu", SHRoles.Personel)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var total = db.GetVWCMP_RequestCount(condition.Filter);
			return total;
		}

		[PageInfo("Satın Alma Talepleri Veri Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_Request(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Satın Alma Talep Detayı", SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.ProjeYonetici, SHRoles.SatinAlmaTalebi, SHRoles.SahaGorevPersonel, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ActionResult Detail(Guid id)
		{
			var data = new VMCMP_RequestModels { id = id }.Load(false);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}
		[PageInfo("Satın Alma Talebi Ekleme", SHRoles.SatinAlmaTalebi, SHRoles.ProjeYonetici, SHRoles.SahaGorevPersonel)]
		public ActionResult Insert(VMCMP_RequestModels item, bool? transform = false)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var model = item.Load(transform);
			if (model.IsCopy != true)
			{
				model.customerId = userStatus.user.CompanyId;
			}
			return View(model);
		}

		[PageInfo("Satın Alma Talebi Ekleme", SHRoles.SatinAlmaTalebi, SHRoles.ProjeYonetici, SHRoles.SahaGorevPersonel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMCMP_RequestModels item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = item.Save(userStatus.user.id, Request);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Satın Alma talebi oluşturma işlemi başarılı", false) :
						   feedback.Warning("Satın Alma talebi oluşturma işlemi başarısız. Mesaj : " + dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Satın Alma Talebini Düzenle", SHRoles.SatinAlmaTalebi, SHRoles.SahaGorevPersonel)]
		public ActionResult Update(Guid id)
		{
			var data = new VMCMP_RequestModels { id = id }.Load(false);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[PageInfo("Satın Alma Talebini Düzenle", SHRoles.SatinAlmaTalebi, SHRoles.SahaGorevPersonel)]
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Update(VMCMP_RequestModels item)
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



		[PageInfo("Satın Alma Talebi Not Ekleme Metodu", SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.ProjeYonetici, SHRoles.SahaGorevPersonel, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ContentResult InsertNote(Guid requestId, string note)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbres = new VMCMP_RequestModels { id = requestId }.Load(false).InsertNote(userStatus.user.id, note);
			return Content(Infoline.Helper.Json.Serialize(dbres), "application/json");
		}

		[PageInfo("Satın Alma Talebi Onay-Red Metodu", SHRoles.SatinAlmaOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaOnaylayiciGorev)]
		public ContentResult UpdateStatus(Guid requestId, int type)
		{
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbres = new VMCMP_RequestModels { id = requestId }.Load(false).UpdateStatus(type, userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbres.result,
				Object = dbres.objects,
				FeedBack = dbres.result ? feedback.Success("İşlem başarılı Mesaj : " + dbres.message, false) :
						   feedback.Warning("İşlem başarısız. Mesaj : " + dbres.message)
			};

			return Content(Infoline.Helper.Json.Serialize(result), "application/json");
		}

		[HttpPost]
		[PageInfo("Satın Alma Talebi Silme Metodu", SHRoles.SatinAlmaTalebi)]
		public JsonResult Delete(Guid id)
		{
			var feedback = new FeedBack();
			var dbresult = new VMCMP_RequestModels { id = id }.Load(false).Delete();

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Satın Alma Talebi Silme Metodu", SHRoles.SatinAlmaTalebi)]
		public JsonResult Cancel(Guid id)
		{
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbresult = new VMCMP_RequestModels { id = id }.Load(false).Cancel(userStatus.user.id);

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("İptal işlemi başarılı") : feedback.Warning(dbresult.message + "İptal işlemi başarısız")
			}, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Satın Alma Talebi Yazdır"), ExportPDF, AllowEveryone]
		public ActionResult Print(Guid id, int? type)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var model = new VMCMP_RequestModels { id = id }.Load(false).GetFormTemplate(type, (userStatus != null ? userStatus.user : new VWSH_User()));
			return View(model);
		}
	}
}
