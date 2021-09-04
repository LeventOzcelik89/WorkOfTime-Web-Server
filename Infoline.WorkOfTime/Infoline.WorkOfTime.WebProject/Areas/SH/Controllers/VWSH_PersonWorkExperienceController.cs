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
    public class VWSH_PersonWorkExperienceController : Controller
	{
        [PageInfo("İş Deneyimi Methodu",SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonWorkExperience(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonWorkExperienceCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Personel İş Deneyimi Detayı", SHRoles.IKYonetici)]
        ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonWorkExperienceById(id);
		    return View(data);
		}

        [PageInfo("Personel İş Deneyimi Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? userId)
		{
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = new VWSH_PersonWorkExperience { id = Guid.NewGuid(),UserId=userId };

            if (userId != null)
            {
                var companyPerson = db.GetINV_CompanyPerson().Where(x => x.IdUser == userId).OrderBy(x => x.JobStartDate).FirstOrDefault();
                if (companyPerson != null)
                {
                    data.JobStartDate = companyPerson.JobStartDate.Value.AddDays(-1);
                    data.JobEndDate = companyPerson.JobStartDate.Value.AddHours(-1);
                }
                
            }
            
            return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel İş Deneyimi Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_PersonWorkExperience item)
		{   
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();


            var companyPerson = db.GetSH_PersonWorkExperience().Where(x => x.UserId == item.UserId && item.JobStartDate > x.JobStartDate && item.JobEndDate < x.JobEndDate  && x.CompanyName == item.CompanyName && x.WorkingPosition == item.WorkingPosition).FirstOrDefault();
            if (companyPerson != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Personel Seçili Tarihler Arasında Aynı Firmada Aynı Pozisyonda Çalışamaz")
                }, JsonRequestBehavior.AllowGet);
            }

            if (db.GetVWSH_PersonWorkExperienceIds((Guid)item.UserId, item.CompanyName, item.WorkingPosition, item.JobStartDate.Value, item.JobEndDate.Value, item.JobDescription) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kayıt Bulunduğundan İşlem Gerçekleşmedi. Tekrar Deneyiniz!!")
                }, JsonRequestBehavior.AllowGet);
            }

            item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
     
            var dbresult = db.InsertSH_PersonWorkExperience(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Personel İş Deneyimi Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonWorkExperienceById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel İş Deneyimi Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonWorkExperience item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
            var companyPerson = db.GetSH_PersonWorkExperience().Where(x => x.id != item.id && x.UserId == item.UserId && item.JobStartDate > x.JobStartDate && item.JobEndDate < x.JobEndDate && x.CompanyName == item.CompanyName && x.WorkingPosition == item.WorkingPosition).FirstOrDefault();
            if (companyPerson != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Personel Seçili Tarihler Arasında Aynı Firmada Aynı Pozisyonda Çalışamaz")
                }, JsonRequestBehavior.AllowGet);
            }
            item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;

            if (db.GetVWSH_PersonWorkExperienceIds((Guid)item.UserId, item.CompanyName, item.WorkingPosition, item.JobStartDate.Value, item.JobEndDate.Value, item.JobDescription) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kayıt Bulunduğundan İşlem Gerçekleşmedi. Tekrar Deneyiniz!!")
                }, JsonRequestBehavior.AllowGet);
            }
            var controlGrid = db.GetVWSH_PersonWorkExperienceIds((Guid)item.UserId, item.CompanyName, item.WorkingPosition, item.JobStartDate.Value, item.JobEndDate.Value, item.JobDescription);
            if (controlGrid != null)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Aynı Kayıt Bulunduğundan İşlem Gerçekleşmedi. Tekrar Deneyiniz!! ") }, JsonRequestBehavior.AllowGet);
            }
            var dbresult = db.UpdateSH_PersonWorkExperience(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Personel İş Deneyimi Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		    var item = id.Select(a => new SH_PersonWorkExperience { id = new Guid(a) });
		    var dbresult = db.BulkDeleteSH_PersonWorkExperience(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
