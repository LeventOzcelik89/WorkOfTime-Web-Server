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
	public class CMP_InvoiceDocumentTemplateController : Controller
	{
		[PageInfo("Satış Teklif Şablonları", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis)]

		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Satış Teklif Şablonları", SHRoles.Personel)]

		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var condition = KendoToExpression.Convert(request);
			condition = VMCMP_InvoiceModels.UpdateQuery(condition, userStatus);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();

			var data = db.GetVWCMP_InvoiceDocumentTemplate(condition).ToDataSourceResult(request);
			data.Total = db.GetVWCMP_InvoiceDocumentTemplateCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Satış Teklif Şablonları", SHRoles.Personel,SHRoles.CRMBayiPersoneli,SHRoles.CagriMerkezi)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var condition = KendoToExpression.Convert(request);
			condition = VMCMP_InvoiceModels.UpdateQuery(condition, userStatus);
			var db = new WorkOfTimeDatabase();

			var data = db.GetVWCMP_InvoiceDocumentTemplate(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Satış Teklif Şablonları", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis)]
		public ActionResult Insert()
		{
			var data = new CMP_InvoiceDocumentTemplateModel { id = Guid.NewGuid() };
			return View(data);
		}

		[PageInfo("Satış Teklif Şablonları", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis)]
		[HttpPost]
		public JsonResult Insert(CMP_InvoiceDocumentTemplateModel item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			var dbresult = db.InsertCMP_InvoiceDocumentTemplate(new CMP_InvoiceDocumentTemplate().B_EntityDataCopyForMaterial(item));
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Satış Teklif Şablonları", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis,SHRoles.CRMBayiPersoneli,SHRoles.CagriMerkezi)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetCMP_InvoiceDocumentTemplateById(id);
			var model = new CMP_InvoiceDocumentTemplateModel().B_EntityDataCopyForMaterial(data);
			return View(model);
		}

		[PageInfo("Satış Teklif Şablonları", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
		[HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
		public JsonResult Update(CMP_InvoiceDocumentTemplateModel item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;


			var dbresult = db.UpdateCMP_InvoiceDocumentTemplate(new CMP_InvoiceDocumentTemplate().B_EntityDataCopyForMaterial(item));
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Satış Teklif Şablonları", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.MuhasebeSatis)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			var item = id.Select(a => new CMP_InvoiceDocumentTemplate { id = new Guid(a) });

			var dbresult = db.BulkDeleteCMP_InvoiceDocumentTemplate(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}

	public class CMP_InvoiceDocumentTemplateModel : CMP_InvoiceDocumentTemplate
	{
		[AllowHtml]
		public string cover { get; set; }
		[AllowHtml]
		public string description { get; set; }
		[AllowHtml]
		public string tenderConditions { get; set; }
	}
}
