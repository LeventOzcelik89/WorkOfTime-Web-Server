using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_PersonLanguagesController : Controller
	{
        [PageInfo("Personel Yabancı Dilleri", SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}


        [PageInfo("Personel Yabancı Dilleri Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonLanguages(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonLanguagesCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Personel Yabancı Dili Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? userId)
		{
		    var data = new VWSH_PersonLanguages { id = Guid.NewGuid(), UserId = userId };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Yabancı Dili Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_PersonLanguages item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
            
            if (db.GetSH_PersonLanguagesByUserIdAndLanguage(item.UserId.Value, item.Languages.Value) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Seçtiğiniz Dil Zaten Kayıtlı")
                }, JsonRequestBehavior.AllowGet);
            }


		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_PersonLanguages(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Yabancı Dili Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonLanguagesById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Yabancı Dili Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonLanguages item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            if (db.GetSH_PersonLanguagesByIdAndUserIdAndLanguage(item.id, item.UserId.Value, item.Languages.Value) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Seçtiğiniz Dil Zaten Kayıtlı")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_PersonLanguages(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Personel Yabancı Dili Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PersonLanguages { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PersonLanguages(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
