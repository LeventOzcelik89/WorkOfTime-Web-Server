using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
	public class VWSV_ChangedDeviceController : Controller
	{
		[PageInfo("Değişen Cihazların Listelendiği Sayfa",SHRoles.TeknikServisYoneticiRolu,SHRoles.TeknikServisBayiRolu)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Değişsen Cihazların Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu,SHRoles.CagriMerkezi)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_ChangedDevice(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSV_ChangedDeviceCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Değişen Cihazların Dropdownda Listelenmesi için gereken metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu, SHRoles.CagriMerkezi)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_ChangedDevice(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		
		[PageInfo("Değişen Cihazların Ayrıntıları", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Detail(Guid id)
		{
			var data = new VMSV_ChangedDeviceModel {id=id }.Load();
			return View(data);
		}

		[PageInfo("Yeni Değişen Cihaz Ekleme Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Insert(VMSV_ChangedDeviceModel model)
		{
		   
		    return View(model);
		}

		[PageInfo("Yeni Değişen Cihaz Ekleme Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMSV_ChangedDeviceModel model, bool? isPost)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
			var dbresult = model.Save(userStatus.user.id);

			var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Cihaz Başarıyla Değiştirildi",true,Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Cihaz Başarıyla Değiştirilme İşlemi Başarısız Oldu")
		    };
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
		
		[PageInfo(" Değişen Cihaz Güncelleme Sayfası", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Update(VMSV_ChangedDeviceModel model)
		{
			var data = new VMSV_ChangedDeviceModel().Load();
			return View(data);
		}

		[PageInfo("Değişen Cihaz Güncelleme Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMSV_ChangedDeviceModel model, bool? isPost)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = model.Save(userStatus.user.id);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Cihaz Başarıyla Değiştirilme Kaydı Güncellendi") : feedback.Warning("Cihaz Başarıyla Değiştirilme Kaydı Güncelleme işlemi başarısız oldu!")
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Değişen Cihaz Silme Metodu", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		[HttpPost]
		public JsonResult Delete(VMSV_ChangedDeviceModel model)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();


			var dbresult = model.Delete();
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Cihaz Başarıyla Değiştirilme Kaydı Silindi") : feedback.Warning("Cihaz Başarıyla Değiştirilme Kaydı Silme İşlemi Başarısız Oldu!")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
