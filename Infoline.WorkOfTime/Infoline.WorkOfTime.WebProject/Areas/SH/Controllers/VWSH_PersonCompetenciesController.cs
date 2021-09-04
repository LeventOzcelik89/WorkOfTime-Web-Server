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
    public class VWSH_PersonCompetenciesController : Controller
	{
        [PageInfo("Personel Yeterlilikleri Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCompetencies(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonCompetenciesCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Yeterliliği Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? userId)
		{
		    var data = new VWSH_PersonCompetencies { id = Guid.NewGuid(),UserId = userId};
          
		    return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Yeterliliği Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_PersonCompetencies item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;

            if (db.GetVWSH_PersonCompetenciesUserAndCompetenciesId((Guid)item.CompetenciesId, (Guid)item.UserId) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kullanıcı veya Yeterlilik Kaydı Bulunduğundan Kayıt Gerçekleşmedi. Tekrar Deneyiniz!! ")
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = db.InsertSH_PersonCompetencies(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Yeterliliği Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCompetenciesById(id);
		    return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Yeterliliği Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonCompetencies item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            if (db.GetVWSH_PersonCompetenciesUserAndCompetenciesIds((Guid)item.id, (Guid)item.CompetenciesId, (Guid)item.UserId) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kullanıcı veya Yeterlilik Kaydı Bulunduğundan Kayıt Gerçekleşmedi. Tekrar Deneyiniz!! ")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_PersonCompetencies(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
        [PageInfo("Personel Yeterliliği Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PersonCompetencies { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PersonCompetencies(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
