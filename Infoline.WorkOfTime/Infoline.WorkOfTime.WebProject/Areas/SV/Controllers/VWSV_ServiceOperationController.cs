using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
	public class VWSV_ServiceOperationController : Controller
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
		    var data = db.GetVWSV_ServiceOperation(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSV_ServiceOperationCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[AllowEveryone]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_ServiceOperation(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[AllowEveryone]
		public ActionResult Detail(Guid id)
		{
		    
		    var data = new VMSV_ServiceOperationModel{id=id };
		    return View(data);
		}

		[AllowEveryone]
		public ActionResult Insert(VMSV_ServiceOperationModel model)
		{
		    return View(model);
		}

		[AllowEveryone]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMSV_ServiceOperationModel model,bool?ispost)
		{
		  
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			var dbresult = model.Save(userStatus.user.id);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Teknik Servis Aksiyon Kaydı Başarıyla Oluşturuldu",false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		public ActionResult Update(VMSV_ServiceOperationModel model)
		{
			var data = new VMSV_ServiceOperationModel().Load();
		    return View(data);
		}

		[AllowEveryone]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMSV_ServiceOperationModel model, bool? ispost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = model.Save(userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SV_ServiceOperation { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSV_ServiceOperation(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		public ActionResult Transfer(VMSV_ServiceOperationModel model)
		{
			return View(model);
		}

		[AllowEveryone]
		public JsonResult NextStage(VMSV_ServiceOperationModel model) {
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			model.description = "Bir Sonraki Aşamaya Geçildi!";
			var dbresult = model.Save(userStatus.user.id);
            if (model.status!= (short)EnumSV_ServiceActions.Done)
            {
				dbresult &= new VMSV_ServiceModel { id = model.serviceId.Value }.NextStage(userStatus.user.id);
			}
	
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Diğer Aşamaya Geçildi", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		public ActionResult Upsert(VMSV_ServiceOperationModel model)
		{
			model.Transaction = new VWPRD_Transaction();
			model.Transaction.type = model.Type;
			return View(model.Load());
		}
		[HttpPost]
		[AllowEveryone]
		public ActionResult Upsert(VMSV_ServiceOperationModel model,bool?isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			if (model.Transaction.type == (int)EnumPRD_TransactionType.HarcamaBildirimi)
			{
				model.description = "Harcama Bildirimi Yapıldı";
			}
			else if (model.Transaction.type == (int)EnumPRD_TransactionType.FireFisi)
			{

				model.description = "Fire Bildirimi Yapıldı";
			}
			var dbresult = model.Upsert(userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(model.description, false, Request.UrlReferrer.AbsoluteUri) : feedback.Error($"{model.description }başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
		[AllowEveryone]
		public JsonResult QualityCheck(Guid serviceId,bool status) {
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = new VMSV_ServiceOperationModel().QualiltyCheck(serviceId,status,userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Aşama Güncellendi", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Güncelleme işlemi başarısız")
			};
			return Json(result,JsonRequestBehavior.AllowGet);

		}
	}
}
