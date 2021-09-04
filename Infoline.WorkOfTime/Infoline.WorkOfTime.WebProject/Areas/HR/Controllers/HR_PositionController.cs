using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HR.Controllers
{
	public class HR_PositionController : Controller
	{
        [PageInfo("Pozisyonlar",SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Pozisyonlar Methodu", SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_Position(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetHR_PositionCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Pozisyonlar Veri Methodu",SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_Position(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Pozisyonlar Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_PositionById(id);
		    return View(data);
		}

        [PageInfo("Pozisyon Ekleme", SHRoles.IKYonetici,SHRoles.PersonelTalebi)]
        public ActionResult Insert()
		{
		    var data = new HR_Position { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Pozisyon Ekleme Methodu", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_Position item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_Position(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Pozisyon Güncelleme" ,SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_PositionById(id);
		    return View(data);
		}

        [PageInfo("CV havuzu personel pozisyonları güncelleme methodudur", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_Position item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_Position(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Pozisyon Silme", SHRoles.IKYonetici)]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new HR_Position { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteHR_Position(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
