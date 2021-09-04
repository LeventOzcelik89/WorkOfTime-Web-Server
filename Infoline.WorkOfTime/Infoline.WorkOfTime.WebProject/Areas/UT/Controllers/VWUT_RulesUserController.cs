using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.UT.Controllers
{
	public class VWUT_RulesUserController : Controller
	{
		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilendirildiği sayfadır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilendirildiği verilerin tutulduğu method.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWUT_RulesUser(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWUT_RulesUserCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilendirildiği dropdown verilerinin tutulduğu method.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeDatabase();
			var data = db.GetVWUT_RulesUser(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilerinin eklendiği sayfadır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Insert(Guid id)
		{
			var ruleUsers = new WorkOfTimeDatabase().GetUT_RulesUserByRuleId(id).Where(x => x.userId.HasValue).Select(x => x.userId.Value).ToArray();
			var data = new VMUT_RulesUser { id = Guid.NewGuid(), rulesId = id };
			return View(data);
		}

		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilerinin eklendiği methoddur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMUT_RulesUser item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			if (!item.rulesId.HasValue)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Kural idsi boş olamaz.")
				}, JsonRequestBehavior.AllowGet);
			}

			if (item.userIds.Count() == 0)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Kullanıcı seçimi yapmalısınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			var rule = db.GetUT_RulesById(item.rulesId.Value);
			if (rule == null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Kural bulunamadı.")
				}, JsonRequestBehavior.AllowGet);
			}


			var rulesUserList = new List<UT_RulesUser>();
			var rulesUserUpdateList = new List<UT_RulesUser>();
			var errorlist = new List<string>();

			var users = db.GetSH_UserByIds(item.userIds);

			foreach (var user in item.userIds)
			{
				var userRules = db.GetVWUT_RulesUserByUserIdAndRulesIdNot(user, item.rulesId.Value);
				if (userRules.Count(x => x.ruleType == rule.type) > 0)
				{
					errorlist.Add(users.Where(x => x.id == user).Select(x => x.firstname + " " + x.lastname).FirstOrDefault());
					continue;
				}

				var oldUserRules = db.GetUT_RulesUserByUserIdAndRulesId(user, item.rulesId.Value);
				if (oldUserRules != null)
				{
					oldUserRules.changed = DateTime.Now;
					oldUserRules.changedby = userStatus.user.id;
					rulesUserUpdateList.Add(oldUserRules);
				}
				else
				{
					rulesUserList.Add(new UT_RulesUser
					{
						createdby = userStatus.user.id,
						rulesId = item.rulesId,
						userId = user
					});
				}
			}

			if (item.userIds.Count() == errorlist.Count())
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Seçmiş olduğunuz tüm kullanıcılara daha önce aynı tipte kural tanımı gerçekleştirilmiş.Lütfen kaldırdıktan sonra tekrar deneyiniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			var trans = db.BeginTransaction();
			var dbresult = db.BulkInsertUT_RulesUser(rulesUserList, trans);
			dbresult &= db.BulkUpdateUT_RulesUser(rulesUserUpdateList, true, trans);

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
				FeedBack = dbresult.result ? feedback.Success("Kullanıcılara kural tanımlama işlemi başarılı" + (errorlist.Count() > 0 ? ".Fakat " + string.Join(",", errorlist) + " kullanıcılarına daha önce aynı kural tipi ile atama yapılmış" : "")) : feedback.Warning("Kullanıcılara kural tanımlama işlemi başarısız oldu.")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilerinin düzenlendiği sayfadır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWUT_RulesUserById(id);
			return View(data);
		}

		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilerinin düzenlendiği methoddur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(UT_RulesUser item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;

			var dbresult = db.UpdateUT_RulesUser(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Kural tanımlamalarının kullanıcılar ile ilişkilerinin silme işleminin gerçekleştirildiği methoddur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			var item = id.Select(a => new UT_RulesUser { id = new Guid(a) });
			//TODO:Oğuz Burada ilişkiler ile ilgili izin var ise silinmemesini sağla
			var dbresult = db.BulkDeleteUT_RulesUser(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
