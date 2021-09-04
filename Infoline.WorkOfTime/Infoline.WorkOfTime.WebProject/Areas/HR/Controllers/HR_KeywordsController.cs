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
    public class HR_KeywordsController : Controller
	{
        [PageInfo("Personel Bilgi & Becerileri", SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Bilgi & Becerileri Methodu", SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_Keywords(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetHR_KeywordsCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Bilgi & Becerileri Veri Methodu",SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_Keywords(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Bilgi & Becerileri Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_KeywordsById(id);
		    return View(data);
		}

        [PageInfo("Bilgi & Becerileri Ekleme", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        public ActionResult Insert()
		{
		    var data = new HR_Keywords { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Bilgi & Becerileri Ekleme Methodu", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_Keywords item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertHR_Keywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

   

        [PageInfo("Bilgi & Becerileri Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetHR_KeywordsById(id);
		    return View(data);
		}

        [PageInfo("Bilgi & Becerileri Güncelleme Methodu", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HR_Keywords item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateHR_Keywords(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Bilgi & Becerileri Silme", SHRoles.IKYonetici)]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new HR_Keywords { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteHR_Keywords(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

	}
}
