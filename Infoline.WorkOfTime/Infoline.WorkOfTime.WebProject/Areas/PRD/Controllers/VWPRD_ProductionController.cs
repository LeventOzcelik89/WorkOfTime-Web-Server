using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductionController : Controller
	{
		[PageInfo("Üretim Emirleri", SHRoles.SistemYonetici)]
		public ActionResult Index()
		{
			return View();
		}


		[PageInfo("Üretim Emri Oluştur", SHRoles.SistemYonetici)]
		public ActionResult Insert(VMPRD_ProductionModel data)
		{
			return View(data.Load());
		}

		[HttpPost]
		[PageInfo("Üretim Emri Oluştur", SHRoles.SistemYonetici)]
		public JsonResult Insert(VMPRD_ProductionModel data, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = new ResultStatus { result = true };

			dbresult = data.Save(userStatus.user.id, Request);

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Üretim emri başarıyla oluşturuldu.") :
											 feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Üretim emri oluşturma işlemi başarısız oldu.")
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Tüm Üretim Emirleri", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var userStatus = (PageSecurity)Session["userStatus"];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_Production(condition).ToDataSourceResult(request);
			data.Total = db.GetVWPRD_ProductionCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		
	}
}