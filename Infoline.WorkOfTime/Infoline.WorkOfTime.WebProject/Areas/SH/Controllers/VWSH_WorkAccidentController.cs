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
using System.Web;
using RazorEngine;
using RazorEngine.Templating;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
	public class VWSH_WorkAccidentController : Controller
	{
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Listesi")]
		public ActionResult Index()
		{
		    return View();
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Veri Methodu")]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccident(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_WorkAccidentCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Dropdown Methodu")]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccident(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Detayı")]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}


		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Ekleme")]
		public ActionResult Insert(VMSH_WorkAccidentModel model)
		{
			var data = model.Load();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Ekleme")]
		public JsonResult Insert(VMSH_WorkAccidentModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay bildirim kaydetme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Güncelleme")]
		public ActionResult Update(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}

		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Bilgileri Güncelleme")]
		public ActionResult UpdateInfo(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}


		[HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Güncelleme")]
		public JsonResult Update(VMSH_WorkAccidentModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(userStatus.user.id);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message, false, Request.UrlReferrer.AbsoluteUri,timeout:1) : feedback.Warning("Kaza ve olay bildirim güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}		
		
		[HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim İçeriği Güncelleme")]
		public JsonResult SaveContent(VMSH_WorkAccidentModel model, bool? isPost)
		{
			var db = new WorkOfTimeDatabase();
			var dbresult = db.UpdateSH_WorkAccident(new SH_WorkAccident().B_EntityDataCopyForMaterial(model));

			var feedback = new FeedBack();
			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("İçerik Başarılı Bir Şekilde Güncellenmiştir.", false, Request.UrlReferrer.AbsoluteUri, timeout: 1) : feedback.Warning("Kaza ve olay bildirim güncelleme işlemi başarısız. Mesaj : " + dbresult.message)
			}, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[AllowEveryone]
		[PageInfo("Kaza Ve Olay Bildirim Silme")]
		public JsonResult Delete(Guid id)
		{
			return Json(new ResultStatusUI(new VMSH_WorkAccidentModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
		}


		[AllowEveryone]
		public string RenderTemplate(Guid? userId, Guid? projectId, Guid? taskId, Guid templateId)
		{
			var db = new WorkOfTimeDatabase();
			var UTtemplate = db.GetUT_TemplateById(templateId);
			string result = HttpUtility.HtmlDecode(UTtemplate.template);
			if (userId.HasValue)
            {
				var user = new VMSH_UserModel() { id = userId.Value }.Load();
				result = GetUserHtml(user,result);
			}
			if (projectId.HasValue)
            {
				var project = db.GetVWPRJ_ProjectById(projectId.Value);
				result = GetProjectHtml(project, result);
			}
			if (taskId.HasValue)
            {
				var task = new VMFTM_TaskModel() { id = taskId.Value }.Load();
				result = GetTaskHtml(task, result);
			}
			var resHtml = HttpUtility.HtmlDecode(result);
			return resHtml;
		}

		private static string GetUserHtml(VMSH_UserModel user, string template)
		{
			template = template.Replace("Personel(Model", "@(Model");
			var result = Engine.Razor.RunCompile(HttpUtility.HtmlDecode(template), string.Format("{0}", Guid.NewGuid()), null, user);
			var resHtml = HttpUtility.HtmlDecode(result);
			return resHtml;
		}		
		private static string GetProjectHtml(VWPRJ_Project project, string template)
		{
			template = template.Replace("Proje(Model", "@(Model");
			var result = Engine.Razor.RunCompile(HttpUtility.HtmlDecode(template), string.Format("{0}", Guid.NewGuid()), null, project);
			var resHtml = HttpUtility.HtmlDecode(result);
			return resHtml;
		}
		private static string GetTaskHtml(VMFTM_TaskModel task, string template)
		{
			template = template.Replace("Görev(Model", "@(Model");
			var result = Engine.Razor.RunCompile(HttpUtility.HtmlDecode(template), string.Format("{0}", Guid.NewGuid()), null, task);
			var resHtml = HttpUtility.HtmlDecode(result);
			return resHtml;
		}

	}
}
