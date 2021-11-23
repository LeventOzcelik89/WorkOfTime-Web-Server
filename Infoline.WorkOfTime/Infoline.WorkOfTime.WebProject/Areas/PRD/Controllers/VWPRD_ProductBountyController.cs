using BotDetect;
using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductBountyController : Controller
	{
		[PageInfo("Ürün Prim Tanımları DataSource", SHRoles.Personel, SHRoles.BayiPersoneli)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductBounty(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWPRD_ProductBountyCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Prim Tanımları Dropdown Metodu", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_ProductBounty(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Prim Tanımı Detayı", SHRoles.StokYoneticisi, SHRoles.UretimYonetici)]
		public ActionResult Detail(VMPRD_ProductBountyModel item)
		{
			var data = item.Load();
			return View(data);
		}

		[PageInfo("Ürün Prim Tanımı Ekleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult Insert(VMPRD_ProductBountyModel item)
		{
			var data = item.Load();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Ürün Tanımı Ekleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli)]
		public JsonResult Insert(VMPRD_ProductBountyModel item, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.Save(userStatus.user.id, Request);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız.Mesaj : " + dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Ürün Tanımı Güncelleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi, SHRoles.SatisPersoneli)]
		public ActionResult Update(VMPRD_ProductBountyModel item)
		{
			var data = item.Load();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Ürün Tanımı Güncelleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult Update(VMPRD_ProductBountyModel item, int[] status)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.Save(userStatus.user.id, Request);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message, false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Ürün Tanımı Silme", SHRoles.StokYoneticisi)]
		public JsonResult Delete(VMPRD_ProductBountyModel item)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var dbresult = item.Delete();
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
