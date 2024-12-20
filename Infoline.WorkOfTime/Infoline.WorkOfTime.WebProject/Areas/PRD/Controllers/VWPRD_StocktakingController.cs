﻿using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_StocktakingController : Controller
	{
		[AllowEveryone]
		public ActionResult Index()
		{
		    return View();
		}

		[AllowEveryone]

		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_Stocktaking(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_StocktakingCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[AllowEveryone]

		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_Stocktaking(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[AllowEveryone]
		public ActionResult Detail(VMPRD_StocktakingModel model)
		{
		    return View(model.Load());
		}

		[AllowEveryone]
		public ActionResult Insert(VMPRD_StocktakingModel model)
		{
		    return View(model.Load());
		}

		[AllowEveryone]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMPRD_StocktakingModel item, bool? isPost)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
			var dbresult = item.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Sayım İşlemi Oluşturma İşlemi Başarılı") : feedback.Warning(dbresult.message),
			}, JsonRequestBehavior.AllowGet);
		}

	    [AllowEveryone]
		[PageInfo("Stok&Envanter İşlem Girişi", SHRoles.Personel, SHRoles.UretimYonetici)]
		public ActionResult TransactionUpsert(VMPRD_StocktakingModel model, VWPRD_Transaction trans, VMPRD_TransactionItems transItem, int? direction)
		{
			model.Transaction = trans;
			model.transactionItem = transItem;
			var data = model.LoadTransaction();
			ViewBag.Direction = direction;
			return View(data);
		}

		[AllowEveryone]
		[PageInfo("Stok&Envanter İşlemi Ekleme ve Güncelleme", SHRoles.UretimYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult TransactionUpsert(VMPRD_StocktakingModel item, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = new ResultStatus { result = true };
			if (item.Transaction.type == (int)EnumPRD_TransactionType.FireFisi)
			{
				dbresult = item.InsertForStockTaking(userStatus.user.id);
			}
			if (item.Transaction.type == (int)EnumPRD_TransactionType.HarcamaBildirimi)
			{
				dbresult = item.InsertForStockTaking(userStatus.user.id);
			}


			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message, false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		public ActionResult Update(VMPRD_StocktakingModel model)
		{
		    return View(model.Load());
		}

		[AllowEveryone]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMPRD_StocktakingModel item, bool? isPost)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Sayım İşlemi Güncelleme İşlemi Başarılı") : feedback.Warning(dbresult.message),
			}, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]

		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new PRD_Stocktaking { id = new Guid(a) });
		
		    var dbresult = db.BulkDeletePRD_Stocktaking(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


	}
}
