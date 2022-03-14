using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
	public class VWFTM_TaskPlanController : Controller
	{
		[PageInfo("Bakım Yönetimi", SHRoles.SahaGorevYonetici)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Planlanmış Görevler DataSource Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWFTM_TaskPlan(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWFTM_TaskPlanCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Planlanmış Görevler DataSourceDropdown Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWFTM_TaskPlan(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Planlanmış Görevler Detayı (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult Detail(Guid id)
		{
			var model = new TaskSchedulerModel();
			model.Load(id);
			return View(model);
		}

		[PageInfo("Planlanmış Görev Detayları (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult TaskDetail(Guid id)
		{
			var model = new TaskSchedulerModel();
			model.Load(id);
			return View(model);
		}

		[PageInfo("Bakım Planları", SHRoles.SahaGorevYonetici)]
		public ActionResult AllTaskDetail()
		{
			return View();
		}

		[PageInfo("Bakım Planları DataSource (New)", SHRoles.SahaGorevYonetici)]
		public ContentResult AllTaskCalendarNewDataSource(int? year, Guid? customerId, Guid? planId)
		{
			var model = new TaskSchedulerModel();

			if (!year.HasValue)
			{
				year = DateTime.Now.Year;
			}

			var res = model.GetTaskTemplatePlanByYearList(year, customerId, planId);
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
			{
				var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
				if (authoritys.Any())
				{
					var tasks = db.GetFTM_TaskByCustomerIds(authoritys.Where(x => x.customerId.HasValue).Select(x => x.customerId.Value).ToArray()).Where(c => c.taskTemplateId.HasValue).Select(c => c.taskTemplateId.Value).ToArray();
					res.TaskPlan = res.TaskPlan.Where(x => tasks.Contains(x.templateId.Value)).ToArray();
				}
			}
			return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = true, objects = res }), "application/json");
		}

		[PageInfo("Bakım Planı Aylara Göre DataSource", SHRoles.SahaGorevYonetici)]
		public ContentResult TaskCalendarYearDataSource(int? year, Guid? customerId, Guid? planId)
		{
			if (!year.HasValue)
			{
				year = DateTime.Now.Year;
			}

			var res = new List<object>();
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var data = new TaskSchedulerModel().TaskPlan.CalendarDataSourceByYear(userStatus, year.Value, customerId, planId);
			var years = data.GroupBy(a => a.end.Year).Select(a => a.Key).OrderBy(a => a).ToArray();

			foreach (var yeary in years)
			{
				var yearData = data.Where(a => a.dueDate.HasValue).ToList();
				res.Add(new VM_TaskTemplateYearList
				{
					Year = yeary,
					Data = yearData
				});
			}
			return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = true, objects = res }), "application/json");
		}


		[PageInfo("Planlanmış Görev ve Şablon Detayı (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult TemplatePlanDetail(Guid id)
		{
			var model = new TaskSchedulerModel();
			model.Load(id);
			return View(model);
		}

		[PageInfo("Planlanmış Görevin Takvim Data Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ContentResult TaskCalendarDataSource(Guid id)
		{
			var res = new List<object>();
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var plan = db.GetVWFTM_TaskPlanById(id);
			var data = new TaskSchedulerModel().TaskPlan.TaskCalendarDataSource(plan, userStatus);
			var years = data.GroupBy(a => a.end.Year).Select(a => a.Key).OrderBy(a => a).ToArray();

			foreach (var year in years)
			{
				var yearData = data.Where(a => a.dueDate.HasValue && a.dueDate.Value.Year == year).ToArray();
				res.Add(new
				{
					Year = year,
					Data = yearData
				});
			}
			return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = true, objects = res }), "application/json");
		}


		[PageInfo("Planlanmış Görev ve Görevlerin Seneleri - Takvim (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ContentResult GetTasksByYear()
		{
			return Content("", "application/json");
		}


		[PageInfo("Planlanmış Görevler Düzenleme (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult Update(Guid id)
		{
			var item = new TaskSchedulerModel();
			item.TaskPlan.id = id;
			item.TaskPlan.Load();
			return View(item);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Planlanmış Görevler Düzenleme (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public JsonResult Update(TaskSchedulerModel item, int? taskCreationTime, int? frequency, int? frequencyInterval)
		{
			item.TaskPlan.frequency = frequency;
			item.TaskPlan.taskCreationTime = taskCreationTime;
			item.TaskPlan.frequencyInterval = frequencyInterval;
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var dbresult = item.TaskPlan.Save(userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Error(dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Planlanmış Görevler Silme (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public JsonResult Delete(Guid id)
		{
			var feedback = new FeedBack();
			var model = new TaskSchedulerModel();
			model.TaskPlan.id = id;
			var dbresult = model.TaskPlan.Delete();

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Error(dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Planlanmış Görevler Yeni Tanımlama (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult Insert()
		{
			var data = new TaskSchedulerModel();
			data.TaskPlan.Load();
			return View(data);
		}

		[HttpPost]
		[PageInfo("Planlanmış Görevler Yeni Tanımlama (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public JsonResult Insert(TaskSchedulerModel data, int? taskCreationTime, int? frequency, int? frequencyInterval)
		{
			data.TaskPlan.frequency = frequency;
			data.TaskPlan.taskCreationTime = taskCreationTime;
			data.TaskPlan.frequencyInterval = frequencyInterval;
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbRes = data.TaskPlan.Save(userStatus.user.id);
			var feedback = new FeedBack();
			var result = new ResultStatusUI
			{
				Result = dbRes.result,
				FeedBack = dbRes.result ? feedback.Success(dbRes.message) : feedback.Error(dbRes.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Planlanmış Görev ve Şablon Tanımla (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult InsertMultiple(Guid? id)
		{
			var model = new TaskSchedulerModel();
			model.Load(id);
			return View(model);
		}

		[HttpPost]
		[PageInfo("Planlanmış Görev ve Şablon Tanımla (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult InsertMultiple(TaskSchedulerModel item, int? taskCreationTime, int? frequency, int? frequencyInterval, short? priority)
		{

			item.TemplateModel.priority = priority;

			item.TaskPlan.frequency = frequency;
			item.TaskPlan.taskCreationTime = taskCreationTime;
			item.TaskPlan.frequencyInterval = frequencyInterval;
			item.TaskPlan.templateId = item.TemplateModel.id;

			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var db = new WorkOfTimeDatabase();
			var trans = db.BeginTransaction();

			var dbresult = item.TemplateModel.Insert(userStatus.user.id, trans);
			dbresult &= item.TaskPlan.Save(userStatus.user.id, trans);

			if (dbresult.result)
			{
				trans.Commit();
			}
			else
			{
				trans.Rollback();
			}

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Error(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);

		}

		[PageInfo("Görev Takvimi", SHRoles.SahaGorevYonetici)]
		public ActionResult Calendar()
		{
			return View();
		}

		[PageInfo("Planlanmış Görevler Takvim Detayı (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ActionResult CalendarDetail(VMFTM_TaskModel request)
		{
			request.Load();
			request.assignableUsersIds = request.assignableUsers;
			return View(request);
		}

		[PageInfo("Planlanmış Görevler Takvim Data Methodu (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
		public ContentResult CalendarDataSource(List<Guid> userIds)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var tasks = new TaskSchedulerModel().TaskPlan.CalendarDataSource(userIds, userStatus);
			return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = true, objects = tasks }), "application/json");
		}
	}
}
