using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.APM.Controllers
{
	public class VWAPM_ActivityActionController : Controller
    {
        [PageInfo("Aktivite Hareketleri", SHRoles.Personel)]
        public ActionResult Index()
		{
		    return View();
		}


        [PageInfo("Aktivite Hareketleri Grid Metodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWAPM_ActivityAction(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWAPM_ActivityActionCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Aktivite Hareketleri Dropdown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWAPM_ActivityAction(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


        [PageInfo("Aktivite Hareket Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWAPM_ActivityActionById(id);
		    return View(data);
		}


        [PageInfo("Aktivite Hareketi Ekleme Sayfası", SHRoles.Personel)]
        public ActionResult Insert()
		{
		    var data = new VWAPM_ActivityAction { id = Guid.NewGuid() };
		    return View(data);
		}


        [PageInfo("Aktivite Hareketi Ekleme Metodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(APM_ActivityAction item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertAPM_ActivityAction(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Aktivite Hareketi Düzenleme Sayfası", SHRoles.Personel)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWAPM_ActivityActionById(id);
		    return View(data);
		}


        [PageInfo("Aktivite Hareketi Düzenleme Metodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(APM_ActivityAction item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateAPM_ActivityAction(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Aktivite Hareketi Silme Metodu", SHRoles.Personel)]
        [HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new APM_ActivityAction { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteAPM_ActivityAction(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
