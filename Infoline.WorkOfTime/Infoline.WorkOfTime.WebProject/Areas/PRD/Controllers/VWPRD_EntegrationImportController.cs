using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_EntegrationImportController : Controller
	{
		[PageInfo("Hak Ediş Raporu Sayfası",SHRoles.Personel)]
		public ActionResult ClaimReport() {

			return View();

		}
		[PageInfo("Hak Ediş Raporu Veri kaynağı", SHRoles.Personel)]
		public JsonResult ClaimReportDataSource(Guid companyId,int year,int month) {
			return Json("", JsonRequestBehavior.AllowGet);

		}





		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_EntegrationImport(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_EntegrationImportCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
		[PageInfo("Entegrasyonda gelen dosya ekleme metodu", SHRoles.Personel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(PRD_EntegrationImport item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertPRD_EntegrationImport(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
