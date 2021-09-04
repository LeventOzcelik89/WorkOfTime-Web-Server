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
	public class VWUT_RulesUserStageController : Controller
	{
		[PageInfo("Kullanıcı kural aşamalarının listelendiği sayfadır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Kullanıcı kural aşamalarının listelendiği methoddur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_RulesUserStage(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWUT_RulesUserStageCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kullanıcı kural aşamalarının listelendiği dropdown methodudur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_RulesUserStage(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Kullanıcı kural aşamalarının detay sayfasıdır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_RulesUserStageById(id);
		    return View(data);
		}

		[PageInfo("Kullanıcı kural aşamalarının ekleme sayfasıdır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Insert(VWUT_RulesUserStage item)
		{
			
		    
		    return View(item);
		}

		[PageInfo("Kullanıcı kural aşamalarının ekleme gerçekleştirildiği methoddur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(UT_RulesUserStage item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			if(!item.rulesId.HasValue || !item.order.HasValue || !item.type.HasValue)
            {
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Lütfen zorunlu alanları doldurun.")
				}, JsonRequestBehavior.AllowGet);
            }

			var rulesStage = db.GetVWUT_RulesUserStageByOrderAndTypeAndRulesId(item.order.Value, item.rulesId.Value);
            if (rulesStage != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı sıra için daha önce kayıt yapılmış.")
				}, JsonRequestBehavior.AllowGet);
			}
            else { 
			var utrulesStages = db.GetVWUT_RulesUserStageByRulesId(item.rulesId.Value);
			if (utrulesStages.Count() == 0)
			{
				if (item.order != 1)
				{
					return Json(new ResultStatusUI
					{
						Result = false,
						FeedBack = feedback.Warning("Önce 1. sıra girilmelidir.")
					}, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				if ((item.order - 1) != utrulesStages.Reverse().FirstOrDefault()?.order)
				{
					return Json(new ResultStatusUI
					{
						Result = false,
						FeedBack = feedback.Warning("Bir önceki sıra " + utrulesStages.Reverse().FirstOrDefault()?.order + " dir.Lütfen 1 artırarak ekleme yapınız.")
					}, JsonRequestBehavior.AllowGet);
				}
			}
			}


			if (item.type == (Int32)EnumUT_RulesUserStage.RoleBagliSecim)
			{
				item.userId = null;
			}
			else
			{
				item.roleId = null;
			}

		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertUT_RulesUserStage(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Kullanıcı kural aşamalarının düzenleme sayfasıdır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWUT_RulesUserStageById(id);
		    return View(data);
		}

		[PageInfo("Kullanıcı kural aşamalarının düzenleme gerçekleştirildiği methoddur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(UT_RulesUserStage item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateUT_RulesUserStage(item,true);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Kullanıcı kural aşamalarının silme methodudur.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new UT_RulesUserStage { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteUT_RulesUserStage(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


	}
}
