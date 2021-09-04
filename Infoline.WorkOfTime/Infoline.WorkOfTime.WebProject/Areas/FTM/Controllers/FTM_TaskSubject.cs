using System;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
	public class FTM_TaskSubjectController : Controller
	{
		[PageInfo("Görev Konusu Tanımları", SHRoles.Personel)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Görev Konuları Methodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetFTM_TaskSubject(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetFTM_TaskSubjectCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		[PageInfo("Görev Konuları Methodu", SHRoles.Personel)]
		public ContentResult DataSourceSubjectType([DataSourceRequest] DataSourceRequest request)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var data = db.GetVWFTM_TaskSubjectType(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWFTM_TaskSubjectTypeCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Görev Konusu Veri Methodu", SHRoles.Personel,SHRoles.BayiGorevPersoneli)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeDatabase();
			var data = db.GetFTM_TaskSubject(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Görev Konusu Detayı", SHRoles.Personel)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetFTM_TaskSubjectById(id);
			return View(data);
		}

		[PageInfo("Görev Konusu Ekleme", SHRoles.Personel)]
		public ActionResult Insert()
		{
			var data = new FTM_TaskSubject { id = Guid.NewGuid() };
			return View(data);
		}

		[PageInfo("Görev Konusu Ekleme Methodu", SHRoles.Personel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(FTM_TaskSubject item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			var dbresult = db.InsertFTM_TaskSubject(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}



		[PageInfo("Görev Konusu Güncelleme", SHRoles.Personel)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetFTM_TaskSubjectById(id);
			return View(data);
		}

		[PageInfo("Görev Konusu Güncelleme Methodu", SHRoles.Personel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(FTM_TaskSubject item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;

			var dbresult = db.UpdateFTM_TaskSubject(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Görev Konusu Silme", SHRoles.Personel)]
		[HttpPost]
		public JsonResult Delete(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			var isItUsed = db.GetFTM_TaskSubjectTypeByTypeId(id);

			if (isItUsed != null)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Bu Görev Konu Tipi Kullanılmaktadır. Kullanılan Görev Konu Tipi Silinemez.") }, JsonRequestBehavior.AllowGet);
			}

			var dbresult = db.DeleteFTM_TaskSubject(id);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}