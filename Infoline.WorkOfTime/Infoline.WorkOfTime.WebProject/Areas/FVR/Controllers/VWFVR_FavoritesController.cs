﻿using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.FVR.Controllers
{
	public class VWFVR_FavoritesController : Controller
	{
		[PageInfo("Favoriler Methodu", SHRoles.Personel, SHRoles.BayiGorevPersoneli)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var userStatus = (PageSecurity)Session["userStatus"];

			var condition = KendoToExpression.Convert(request);
			condition = UpdateQuery(condition, userStatus);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWFVR_Favorites(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWFVR_FavoritesCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Favoriler Veri Methodu", SHRoles.Personel, SHRoles.BayiGorevPersoneli)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFVR_Favorites(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Favori Ekleme", SHRoles.Personel, SHRoles.BayiGorevPersoneli), AllowEveryone]
		public ActionResult Insert(VMFVR_FavoriteModel model)
		{
			model.Action = model.url;
			model.Description = model.title;
			return View(model);
		}

		[PageInfo("Favori Ekleme", SHRoles.Personel, SHRoles.BayiGorevPersoneli), AllowEveryone]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMFVR_FavoriteModel item, bool? isPost)
		{
			item.Action = item.url;
			item.Description = item.title;
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var data = db.GetVWFVR_FavoritesById(item.id);
			var res = new ResultStatus { result = true };
			var result = new ResultStatusUI { };
			if (data == null)
			{
				item.createdby = userStatus.user.id;
				item.created = DateTime.Now;
				item.Status = true;
				item.userId = userStatus.user.id;
				var dbresult = db.InsertFVR_Favorites(new FVR_Favorites().EntityDataCopyForMaterial(item));
				result = new ResultStatusUI
				{
					Result = dbresult.result,
					FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
				};
			}
			else
			{
				item.changedby = userStatus.user.id;
				item.changed = DateTime.Now;
				item.Status = true;
				var dbresult = db.UpdateFVR_Favorites(new FVR_Favorites().EntityDataCopyForMaterial(item));
				result = new ResultStatusUI
				{
					Result = dbresult.result,
					FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
				};
			}
			
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Favori Sil", SHRoles.Personel, SHRoles.BayiGorevPersoneli)]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new FVR_Favorites { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteFVR_Favorites(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
		{
			BEXP filter = null;
			filter &= new BEXP
			{
				Operand1 = (COL)"userId",
				Operator = BinaryOperator.Equal,
				Operand2 = (VAL)userStatus.user.id
			};
			filter &= new BEXP
			{
				Operand1 = (COL)"status",
				Operator = BinaryOperator.Equal,
				Operand2 = (VAL)string.Format("{0}", true)
			};
			query.Filter &= filter;
			return query;
		}
	}

	public class VMFVR_FavoriteModel : VWFVR_Favorites
	{
		public string title { get; set; }
		public string url { get; set; }
	}

}
