using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_SchoolsController : Controller
	{
        [PageInfo("Okul Bilgileri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_Schools(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_SchoolsCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Okul Bilgileri Veri Methodu", SHRoles.IKYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWSH_Schools(condition);
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }

        [PageInfo("Okul Bilgisi Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(VWSH_Schools model)
		{
		    return View(model);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Okul Bilgisi Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_Schools item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_Schools(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
