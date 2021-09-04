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
    public class VWHR_StaffNeedsPositionController : Controller
	{
        [PageInfo("Talep Edilen Personel Pozisyonları")]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Talep Edilen Personel Pozisyonları Methodu")]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsPosition(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWHR_StaffNeedsPositionCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Talep Edilen Personel Pozisyonları Veri Methodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsPosition(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Talep Edilen Personel Pozisyonları Detayı")]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsPositionById(id);
		    return View(data);
		}

        [PageInfo("Talep Edilen Personel Pozisyonu Ekleme")]
        public ActionResult Insert()
		{
		    var data = new VWHR_StaffNeedsPosition { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Talep Edilen Personel Pozisyonu Ekleme")]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_StaffNeedsPosition item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_StaffNeedsPosition(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Talep Edilen Personel Pozisyonu Güncelleme")]

        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWHR_StaffNeedsPositionById(id);
		    return View(data);
		}

        [PageInfo("Talep Edilen Personel Pozisyonu Güncelleme")]

        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_StaffNeedsPosition item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_StaffNeedsPosition(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Talep Edilen Personel Pozisyonu Silme")]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new HR_StaffNeedsPosition { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteHR_StaffNeedsPosition(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
