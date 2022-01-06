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
	public class VWSV_DeviceCameWithController : Controller
	{
		[PageInfo("Cihazın Yanında Gelen Eşyaların Listelendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Index()
		{
		    return View();
		}


		[PageInfo("Cihazın Yanında Gelen Eşyaların Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_DeviceCameWith(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSV_DeviceCameWithCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Cihazın Yanında Gelen Eşyaların Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_DeviceCameWith(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Cihazın Yanında Gelen Eşyaların Detayı", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_DeviceCameWithById(id);
		    return View(data);
		}

		[PageInfo("YEni Cihazın Yanında Gelen Eşyaların Eklendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		public ActionResult Insert()
		{
		    var data = new VWSV_DeviceCameWith { id = Guid.NewGuid() };
		    return View(data);
		}
		[PageInfo("YEni Cihazın Yanında Gelen Eşyaların Eklendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(SV_DeviceCameWith item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSV_DeviceCameWith(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo(" Cihazın Yanında Gelen Eşyaların Güncellendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]

		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSV_DeviceCameWithById(id);
		    return View(data);
		}

		[PageInfo(" Cihazın Yanında Gelen Eşyaların Güncellendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SV_DeviceCameWith item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSV_DeviceCameWith(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo(" Cihazın Yanında Gelen Eşyaların Silindiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SV_DeviceCameWith { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSV_DeviceCameWith(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
