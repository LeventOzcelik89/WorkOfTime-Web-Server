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
    public class VWHR_PersonPositionController : Controller
	{
        [PageInfo("CV Havuzu Pozisyonları")]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("CV Havuzu Pozisyonları Methodu")]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonPosition(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWHR_PersonPositionCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("CV Havuzu Pozisyonları Veri Methodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonPosition(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("CV Havuzu Pozisyonları Detayı")]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonPositionById(id);
		    return View(data);
		}

        [PageInfo("CV Havuzu Pozisyonları Ekleme")]
        public ActionResult Insert()
		{
		    var data = new VWHR_PersonPosition { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("CV havuzu personel pozisyonları ekleme methodu")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_PersonPosition item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_PersonPosition(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
        [PageInfo("CV Havuzu Pozisyonları Güncellemesi")]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonPositionById(id);
		    return View(data);
		}

        [PageInfo("CV havuzu personel pozisyonları güncelleme methodudur")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_PersonPosition item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_PersonPosition(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("CV Havuzu Pozisyon Silme")]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new HR_PersonPosition { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteHR_PersonPosition(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
