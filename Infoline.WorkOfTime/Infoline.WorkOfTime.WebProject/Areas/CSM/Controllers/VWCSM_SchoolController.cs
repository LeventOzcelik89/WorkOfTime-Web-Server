using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CSM.Controllers
{
	public class VWCSM_SchoolController : Controller
    {
        [PageInfo("Öğrenci Okulları", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Index()
		{
		    return View();
		}


        [PageInfo("Okullar Methodu", SHRoles.AdayOgrenciYonetim)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCSM_School(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWCSM_SchoolCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Okullar Dropdown Methodu", SHRoles.Personel, SHRoles.AdayOgrenciYonetim,SHRoles.AdayOgrenciAgent)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCSM_School(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
        
        [PageInfo("Okul Ekleme Sayfası", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Insert()
		{
		    var data = new VWCSM_School { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Okul Ekleme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(CSM_School item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertCSM_School(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Okul Düzenleme Sayfası", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCSM_SchoolById(id);
		    return View(data);
		}

        [PageInfo("Okul Düzenleme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(CSM_School item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateCSM_School(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Okul Silme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new CSM_School { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteCSM_School(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
