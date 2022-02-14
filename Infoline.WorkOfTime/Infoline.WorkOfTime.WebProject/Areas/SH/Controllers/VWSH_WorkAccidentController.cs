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
		[PageInfo("Kaza Ve Olay Bildirim Listesi", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Kaza Ve Olay Bildirim Veri Methodu", SHRoles.Personel, SHRoles.SistemYonetici)]
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


		[PageInfo("Kaza Ve Olay Bildirim Dropdown Methodu", SHRoles.Personel, SHRoles.SistemYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_WorkAccident(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kaza Ve Olay Bildirim Detayı", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici)]
		public ActionResult Detail(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}


		[PageInfo("Kaza Ve Olay Bildirim Ekleme",SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public ActionResult Insert(VMSH_WorkAccidentModel model)
		{
			var data = model.Load();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Kaza Ve Olay Bildirim Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
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

		[PageInfo("Kaza Ve Olay Bildirim Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public ActionResult Update(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}

		[PageInfo("Kaza Ve Olay Bildirim Bilgileri Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
		public ActionResult UpdateInfo(Guid id)
		{
			return View(new VMSH_WorkAccidentModel { id = id }.Load());
		}


		[HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
		[PageInfo("Kaza Ve Olay Bildirim Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
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
		[PageInfo("Kaza Ve Olay Bildirim İçeriği Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
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
		[PageInfo("Kaza Ve Olay Bildirim Silme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
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

			var user = new VMSH_UserModel() { id = userId ?? Guid.NewGuid() }.Load();
			result = GetUserHtml(user,result);

			if (projectId.HasValue)
            {
				var project = db.GetVWPRJ_ProjectById(projectId.Value);
				result = GetProjectHtml(project, result);
            }
            else
            {
				result = GetProjectHtml(new VWPRJ_Project { ProjectCode = "-"}, result);
			}

			var task = new VMFTM_TaskModel() { id = taskId ?? Guid.NewGuid(), code = taskId.HasValue ? null : "-" }.Load();
			result = GetTaskHtml(task, result);

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
