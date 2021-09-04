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
    public class VWHR_StaffNeedsKeywordsController : Controller
	{
        [PageInfo("Personel Bilgi ve Becerileri")]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Personel Bilgi ve Becerileri Methodu")]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsKeywords(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWHR_StaffNeedsKeywordsCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Personel Bilgi ve Becerileri Veri Methodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsKeywords(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Personel Bilgi ve Becerileri Detayı")]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsKeywordsById(id);
		    return View(data);
		}

        [PageInfo("Personel Bilgi ve Becerileri Ekleme")]
        public ActionResult Insert()
		{
		    var data = new VWHR_StaffNeedsKeywords { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Personel Bilgi ve Becerileri Ekleme")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_StaffNeedsKeywords item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_StaffNeedsKeywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Bilgi ve Becerileri Güncelleme")]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsKeywordsById(id);
		    return View(data);
		}

        [PageInfo("Personel Bilgi ve Becerileri Güncelleme")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_StaffNeedsKeywords item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_StaffNeedsKeywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Bilgi ve Becerileri Silme")]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new HR_StaffNeedsKeywords { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteHR_StaffNeedsKeywords(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
