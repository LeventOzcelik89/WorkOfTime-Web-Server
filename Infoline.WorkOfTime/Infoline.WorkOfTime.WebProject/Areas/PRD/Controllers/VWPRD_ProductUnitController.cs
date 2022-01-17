using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductUnitController : Controller
	{
		[PageInfo("Ürüne Birim Tanımla", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult Insert(PRD_ProductUnit data)
		{
			data.isDefault = data.isDefault ?? (int)EnumPRD_ProductUnitIsDefault.Hayir;
			return View(data);
		}

		[HttpPost]
		[PageInfo("Ürüne Birim Tanımla", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult Insert(PRD_ProductUnit data, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var db = new WorkOfTimeDatabase();
			var dbresult = new ResultStatus { result = true };

			var isExistsDefault = db.GetPRD_ProductUnitIsDefaultCount((int)EnumPRD_ProductUnitIsDefault.Evet);
			if (isExistsDefault > 0)
			{
				return Json(new ResultStatusUI
				{
					FeedBack =  feedback.Warning("Varsayılan ürün birimi mevcuttur."),
				}, JsonRequestBehavior.AllowGet);
			}

			data.createdby = userStatus.user.id;
			data.created = DateTime.Now;

			dbresult &= db.InsertPRD_ProductUnit(data);

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız.Mesaj : " + dbresult.message),
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Ürüne Birimi Güncelleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetPRD_ProductUnitById(id);
			return View(data);
		}

		[PageInfo("Ürüne Birimi Güncelleme Methodu", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(PRD_ProductUnit item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();


			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;

			var dbresult = db.UpdatePRD_ProductUnit(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Ürün Birimi Detay", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetPRD_ProductUnitById(id);
			return View(data);
		}


		[PageInfo("Tüm Ürün Birimleri", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var userStatus = (PageSecurity)Session["userStatus"];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductUnit(condition).ToDataSourceResult(request);
			data.Total = db.GetVWPRD_ProductUnitCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Tüm Ürün Birimleri (Dropdown)", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductUnit(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Ürün Birimi Sil", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		[HttpPost]
		public JsonResult Delete(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var res = new ResultStatus { result = true };
			var feedback = new FeedBack();
			var data = db.GetPRD_ProductUnitById(id);

			if (data.isDefault == (int)EnumPRD_ProductUnitIsDefault.Evet)
			{
				return Json(new ResultStatusUI { FeedBack = feedback.Warning("Varsayılan ürün birimi silinemez.")}, JsonRequestBehavior.AllowGet);
			}

			res &= db.DeletePRD_ProductUnit(data);

			var result = new ResultStatusUI
			{
				Result = res.result,
				FeedBack = res.result == false ? feedback.Warning(res.message) : feedback.Success("Ürün birimi silme işlemi başarılı")
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}