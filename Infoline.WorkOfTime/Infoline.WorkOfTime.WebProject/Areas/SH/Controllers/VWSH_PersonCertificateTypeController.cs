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
    public class VWSH_PersonCertificateTypeController : Controller
	{
        [PageInfo("Sertifika Tipleri", SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}
        
        [PageInfo("Sertifika Tipleri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCertificateType(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonCertificateTypeCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Sertifika Tipleri Veri Methodu", SHRoles.IKYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWSH_PersonCertificateType(condition);
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }

        [PageInfo("Sertifika Tipleri Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCertificateTypeById(id);
		    return View(data);
		}


        [PageInfo("Sertifika Tipi Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(VWSH_PersonCertificateType model)
		{
		    return View(model);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Sertifika Tipi Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(SH_PersonCertificateType item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            if (db.GetVWSH_PersonCertificateTypeName(item.CertificateName) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Sertifika Tip Kaydı Bulunduğundan Kayıt Gerçekleşmedi. Tekrar Deneyiniz!! ")
                }, JsonRequestBehavior.AllowGet);
            }

            item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_PersonCertificateType(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Sertifika Tipi Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCertificateTypeById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Sertifika Tipi Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonCertificateType item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

            if (db.GetVWSH_PersonCertificateTypeName(item.CertificateName) != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Sertifika Tip Kaydı Bulunduğundan Kayıt Gerçekleşmedi. Tekrar Deneyiniz!! ")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_PersonCertificateType(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Sertifika Tipi Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PersonCertificateType { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PersonCertificateType(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
