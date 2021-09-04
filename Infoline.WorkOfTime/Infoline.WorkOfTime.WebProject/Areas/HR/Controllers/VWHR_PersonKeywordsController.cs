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
    public class VWHR_PersonKeywordsController : Controller
	{
        [PageInfo("Personel Bilgi ve Becerileri")]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Bilgi ve Becerileri Methodu")]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonKeywords(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWHR_PersonKeywordsCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Bilgi ve Becerileri Veri Methodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonKeywords(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Bilgi ve Becerileri Detayı")]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonKeywordsById(id);
		    return View(data);
		}

        [PageInfo("Bilgi ve Becerileri Ekleme")]
        public ActionResult Insert()
		{
		    var data = new VWHR_PersonKeywords { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Personel bilgi ve becerileri ekleme methodudur")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_PersonKeywords item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_PersonKeywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Bilgi ve Becerileri Güncelleme")]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_PersonKeywordsById(id);
		    return View(data);
		}

        [PageInfo("Personel bilgi ve becerileri güncelleme methodudur")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_PersonKeywords item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_PersonKeywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Bilgi ve Becerileri Silme")]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		    var item = id.Select(a => new HR_PersonKeywords { id = new Guid(a) });
		    var dbresult = db.BulkDeleteHR_PersonKeywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
