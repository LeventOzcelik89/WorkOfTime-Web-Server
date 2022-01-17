using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
	public class VWSV_ServiceOperationController : Controller
	{

		[PageInfo("Servis Operasyonlarının Listelendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Index()
		{
		    return View();
		}


		[PageInfo("Servis Operasyonlarının Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
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

		[PageInfo("Servis Operasyonlarının Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_ServiceOperation(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Servis Operasyonlarının Detaylarının Olduğu Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Detail(Guid id)
		{
		    
		    var data = new VMSV_ServiceOperationModel{id=id };
			return View(data.Load());
		}

		[PageInfo("Servis Operasyonlarının Eklendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Insert(VMSV_ServiceOperationModel model)
		{
		    return View(model);
		}

		[PageInfo("Servis Operasyonlarının Eklendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMSV_ServiceOperationModel model,bool?state)
		{
		  
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
            if (model.status==(int)EnumSV_ServiceOperation.QualityControlNot)
            {
				var dbresult = new VMSV_ServiceOperationModel { description=model.description }.QualiltyCheck(model.serviceId.Value, false, userStatus.user.id);
				var result = new ResultStatusUI
				{
					Result = dbresult.result,
					FeedBack = dbresult.result ? feedback.Success("Teknik Servis Aksiyon Kaydı Başarıyla Oluşturuldu", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Kaydetme işlemi başarısız")
				};
				return Json(result, JsonRequestBehavior.AllowGet);
			}
            else
            {
				var dbresult = model.Save(userStatus.user.id);
				var result = new ResultStatusUI
				{
					Result = dbresult.result,
					FeedBack = dbresult.result ? feedback.Success("Teknik Servis Aksiyon Kaydı Başarıyla Oluşturuldu", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Kaydetme işlemi başarısız")
				};
				return Json(result, JsonRequestBehavior.AllowGet);
			}
			
		   
		
		   
		}

		[PageInfo("Servis Operasyonlarının Güncellendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Update(VMSV_ServiceOperationModel model)
		{
			var data = new VMSV_ServiceOperationModel().Load();
		    return View(data);
		}


		[PageInfo("Servis Operasyonlarının Güncellendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
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

		[PageInfo("Servis Operasyonlarının Silindiği Met", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
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

		[PageInfo("Servise Gelen Cihazın Taşındığı Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Transfer(VMSV_ServiceOperationModel model)
		{
			return View(model);
		}

		[PageInfo("Servis Operasyonlarının Güncellendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public JsonResult NextStage(VMSV_ServiceOperationModel model) {
			var db = new WorkOfTimeDatabase();
			var trans = db.BeginTransaction();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var getService = db.GetVWSV_ServiceById(model.serviceId.Value);
			var stages = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<Infoline.WorkOfTime.BusinessAccess.EnumSV_ServiceStages>();
			
			model.description = $"{stages.Where(x=>(Convert.ToInt32(x.Key)==getService.stage+1)).FirstOrDefault().Value} Aşamasına Geçildi";
			var dbresult = model.Save(userStatus.user.id,null,trans);
            if (model.status!= (short)EnumSV_ServiceActions.Done)
            {
				dbresult &= new VMSV_ServiceModel { id = model.serviceId.Value }.NextStage(userStatus.user.id,trans);
			}
            if (dbresult.result)
            {
				trans.Commit();
            }
            else
            {
				trans.Rollback();
            }
			getService = db.GetVWSV_ServiceById(model.serviceId.Value);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success($"{getService.stage_Title} Aşamasına Geçildi", false, Request.UrlReferrer.AbsoluteUri) : feedback.Error("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Fire ve Harcama Bildirimi Yapan Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		public ActionResult Upsert(VMSV_ServiceOperationModel model)
		{
			model.Transaction = new VWPRD_Transaction();
			model.Transaction.type = model.Type;
			return View(model.Load());
		}
		[HttpPost]
		[PageInfo("Fire ve Harcama Bildirimi Yapan Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		public ActionResult Upsert(VMSV_ServiceOperationModel model,bool?isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			if (model.Transaction.type == (int)EnumPRD_TransactionType.HarcamaBildirimi)
			{
				model.description = "Harcama Bildirimi ";
			}
			else if (model.Transaction.type == (int)EnumPRD_TransactionType.FireFisi)
			{

				model.description = "Fire Bildirimi ";
			}
			var dbresult = model.Upsert(userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(model.description+" Başarılı!", false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning($"{model.description } </br> {dbresult.message} Başarısız!")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
		[PageInfo("Kalite Kontrolun Yapıldığı Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		public JsonResult QualityCheck(Guid serviceId,bool status) {
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = new VMSV_ServiceOperationModel().QualiltyCheck(serviceId,status,userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Aşama Güncellendi", false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Güncelleme işlemi başarısız")
			};
			return Json(result,JsonRequestBehavior.AllowGet);

		}
		[PageInfo("Servisin  Müşteriye Teslim Edilen Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		public ActionResult Cargo(VMSV_ServiceOperationModel model)
		{
			return View(model);
		}
		[HttpPost]
		[PageInfo("Servisin  Müşteriye Teslim Edilen", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		public JsonResult Cargo(VMSV_ServiceOperationModel model,bool?isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = model.Cargo(userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Süreç Tamamlandı", false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Kargo Bilgileri Tanımlama  işlemi başarısız")
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
