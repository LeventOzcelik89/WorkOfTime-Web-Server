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
    public class VWSH_PersonReferencesController : Controller
	{
        [PageInfo("Personel Referansları", SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}


        [PageInfo("Personel Referansları Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonReferences(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonReferencesCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Personel Referansı Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? userId)
		{
		    var data = new VWSH_PersonReferences { id = Guid.NewGuid(), UserId = userId };
		    return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Referansı Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_PersonReferences item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            var control = PersonControlStep(item, feedback, db);
            if (control != null)
            {
                return Json(control, JsonRequestBehavior.AllowGet);
            }


            if (db.GetSH_PersonReferencesByAllItem(item.UserId.Value, item.ReferenceMail, item.ReferencePhone, item.ReferencePosition, item.ReferenceUserName, item.ReferenceWorkingCompany) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Eklemek İstediğiniz Referans Zaten Kayıtlı")
                }, JsonRequestBehavior.AllowGet);
            }
            item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_PersonReferences(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Referansı Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonReferencesById(id);
		    return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Referansı Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonReferences item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            var control = PersonControlStep(item, feedback, db);
            if (control != null)
            {
                return Json(control, JsonRequestBehavior.AllowGet);
            }

            if (db.GetSH_PersonReferencesByIdAndAllItem(item.id,item.UserId.Value,item.ReferenceMail,item.ReferencePhone,item.ReferencePosition,item.ReferenceUserName,item.ReferenceWorkingCompany)!= null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Eklemek İstediğiniz Referans Zaten Kayıtlı")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_PersonReferences(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel Referansı Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_PersonReferencesById(id);
            return View(data);
        }

		[HttpPost]
        [PageInfo("Personel Referansı Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PersonReferences { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PersonReferences(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Kişisel Kontrol Adımları", SHRoles.IKYonetici)]
        public ResultStatusUI PersonControlStep(SH_PersonReferences item, FeedBack feedback, WorkOfTimeDatabase db)
        {
            if (!item.UserId.HasValue)
            {
                return new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kişisel Bilgilere Ulaşılamıyor. Lütfen Daha Sonra Tekrar Deneyiniz..")
                };
            }

            var sh_user = db.GetSH_UserById(item.UserId.Value);
            if (sh_user == null)
            {
                return new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kişisel Bilgilere Ulaşılamıyor. Lütfen Daha Sonra Tekrar Deneyiniz..")
                };
            }

            if (sh_user.email == item.ReferenceMail || item.ReferencePhone == sh_user.cellphone)
            {
                return new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Referans Verilen kişinin bilgileri, personelin şahsi bilgileri olamaz..")
                };
            }

            return null;
        }
    }
}
