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
    public class VWSH_PersonSchoolsController : Controller
	{

        [PageInfo("Personel Okulları Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonSchools(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonSchoolsCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Personel Okulu Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? userId)
		{
		    var data = new VWSH_PersonSchools { id = Guid.NewGuid(), UserId = userId };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Okulu Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_PersonSchools item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();           
            if(item.SchoolId != null && item.UserId != null) { 
            var _control = db.GetSH_PersonSchoolsByInsert(item.SchoolId.Value,item.UserId.Value, item.Branch);

            if (_control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kayıt Zaten Bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }
            }

            item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
            var rsfile = new FileUploadSave(Request).SaveAs();
            if (rsfile.Result == false)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Dosya yükleme işlemi gerçekleşirken problem oluştu."),
                }
                , JsonRequestBehavior.AllowGet);
            }

            var dbresult = db.InsertSH_PersonSchools(item);
            var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Personel Okulu Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonSchoolsById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Okulu Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonSchools item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            if(item.SchoolId!= null && item.UserId != null) { 
            var _control = db.GetSH_PersonSchoolsByParamsUpdate(item.id, item.UserId.Value,item.SchoolId.Value, item.Branch);
            if (_control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kayıt Zaten Bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }
            }


            item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		

            var rsfile = new FileUploadSave(Request).SaveAs();
            if(rsfile.Result == false)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Dosya yükleme işlemi gerçekleşirken problem oluştu."),
                }
                ,JsonRequestBehavior.AllowGet);
            }

            var dbresult = db.UpdateSH_PersonSchools(item);
           
            var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Personel Okulu Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PersonSchools { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PersonSchools(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        
        [PageInfo("Personel Okul Şubeleri Methodu", SHRoles.IKYonetici)]
        public ContentResult BranchGroup()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_PersonSchools().GroupBy(x => x.Branch);
            var veri = data.Where(x => x.Key != null).Select(c => c.Key).ToArray();
            return Content(Infoline.Helper.Json.Serialize(veri),"application/json");
        }
    }
}
