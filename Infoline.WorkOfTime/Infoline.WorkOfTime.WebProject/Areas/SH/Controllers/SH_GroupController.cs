using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH
{
    public class SH_GroupController : Controller
	{
        [PageInfo("Grup / Ekip Tanımlamaları", SHRoles.Personel)]
        public ActionResult Index()
		{
			return View();
		}

        [PageInfo("Grup & Ekip Tanımlamaları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetSH_Group(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetSH_GroupCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Grup & Ekip Tanımlamaları Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetSH_Group(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Grup & Ekip Tanımlamaları Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
			var data = db.GetSH_GroupById(id);
			return View(data);
		}

        [PageInfo("Grup & Ekip Tanımlamaları Ekleme", SHRoles.Personel)]
        public ActionResult Insert()
		{
		    var data = new SH_Group { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Grup & Ekip Tanımlamaları Methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(SH_Group item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;

			var isCodeControl = db.GetSH_GroupByCode(item.code);

            if (isCodeControl != null)
            {
				return Json(new ResultStatusUI {Result = false,FeedBack = feedback.Warning("Bu kod daha önce kullanılmıştır.") }, JsonRequestBehavior.AllowGet);
			}


			var dbresult = db.InsertSH_Group(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Grup & Ekip Tanımlamaları Güncelleme", SHRoles.Personel)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetSH_GroupById(id);
		    return View(data);
		}

        [PageInfo("Grup & Ekip Tanımlamaları Güncelleme Methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SH_Group item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_Group(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Grup & Ekip Tanımlamaları Silme", SHRoles.Personel)]
        [HttpPost]
		public JsonResult Delete(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();


			var group = db.GetSH_GroupById(id);
			if(group == null)
            {
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Grup bulunamadı.")
				},JsonRequestBehavior.AllowGet);
            }

			var groupUser = db.GetSH_GroupUsersByGroupId(group.id);

			var trans = db.BeginTransaction();
			var dbresult = db.DeleteSH_Group(group,trans);
			dbresult &= db.BulkDeleteSH_GroupUsers(groupUser,trans);

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
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
