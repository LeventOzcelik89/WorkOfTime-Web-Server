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
		public ActionResult Insert(VWFTM_TaskAuthority item)
		{
			item.id = Guid.NewGuid();
		    return View(item);
		}

		[PageInfo("Görev Yetki Tanımlama Ekleme Methodu", SHRoles.ProjeYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(FTM_TaskAuthority item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            if (!item.userId.HasValue)
            {
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Personel seçimi yapınız.")
				}, JsonRequestBehavior.AllowGet);
            }

			if (!item.customerId.HasValue)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Müşteri seçimi yapınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			var control = db.GetFTM_TaskAuthorityByUserIdAndCustomerId(item.userId.Value, item.customerId.Value);
			if(control != null)
            {
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı personel ve müşteri daha önce tanımlanmış.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertFTM_TaskAuthority(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Görev yetki tanımlama işlemi başarılı") : feedback.Error("Görev yetki tanımlama işlemi başarısız oldu.")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Görev Yetki Tanımlama Düzenleme Sayfası", SHRoles.ProjeYonetici)]
		public ActionResult Update(VWFTM_TaskAuthority item)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWFTM_TaskAuthorityById(item.id);
		    return View(data);
		}

		[PageInfo("Görev Yetki Tanımlama Düzenleme Methodu", SHRoles.ProjeYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(FTM_TaskAuthority item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			if (!item.userId.HasValue)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Personel seçimi yapınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			if (!item.customerId.HasValue)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Müşteri seçimi yapınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			var control = db.GetFTM_TaskAuthorityByUserIdAndCustomerIdAndNotId(item.userId.Value, item.customerId.Value,item.id);
			if (control != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı personel ve müşteri daha önce tanımlanmış.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateFTM_TaskAuthority(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Görev yetki düzenleme işlemi başarılı") : feedback.Error("Görev yetki düzenleme işlemi başarısız oldu.")
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
