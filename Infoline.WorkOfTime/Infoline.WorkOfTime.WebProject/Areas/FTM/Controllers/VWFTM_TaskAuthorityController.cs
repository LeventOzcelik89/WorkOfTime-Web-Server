using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
    public class VWFTM_TaskAuthorityController : Controller
	{
		[PageInfo("Görev Yetki Tanımlama Sayfası",SHRoles.ProjeYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Görev Yetki Tanımlama Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskAuthority(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWFTM_TaskAuthorityCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskAuthority(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskAuthorityById(id);
		    return View(data);
		}

		[PageInfo("Görev Yetki Tanımlama Ekleme Sayfası", SHRoles.ProjeYonetici)]
		public ActionResult Insert()
		{
		    var data = new VWFTM_TaskAuthority { id = Guid.NewGuid() };
		    return View(data);
		}

		[PageInfo("Görev Yetki Tanımlama Ekleme Methodu", SHRoles.ProjeYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(FTM_TaskAuthority item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertFTM_TaskAuthority(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Görev Yetki Tanımlama Düzenleme Sayfası", SHRoles.ProjeYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskAuthorityById(id);
		    return View(data);
		}

		[PageInfo("Görev Yetki Tanımlama Düzenleme Methodu", SHRoles.ProjeYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(FTM_TaskAuthority item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateFTM_TaskAuthority(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new FTM_TaskAuthority { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteFTM_TaskAuthority(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
