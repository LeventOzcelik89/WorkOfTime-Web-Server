﻿using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_PersonCertificateController : Controller
	{
        [PageInfo("Personel Sertifikaları", SHRoles.IKYonetici)]
        public ActionResult Index()
		{
		    return View();
		}

        [PageInfo("Personel Sertifikaları Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCertificate(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSH_PersonCertificateCount(condition.Filter);
		    return Json(data, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Personel Sertifikaları Methodu 2", SHRoles.Personel)]
		public JsonResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWSH_PersonCertificate(condition);
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Personel Sertifikaları Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCertificateById(id);
		    return View(data);
		}


        [PageInfo("Personel Sertifikası Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? userId)
		{
		    var data = new VWSH_PersonCertificate { id = Guid.NewGuid(), UserId = userId};
		    return View(data);
		}


        [PageInfo("Personel Sertifikası Ekleme", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_PersonCertificate item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertSH_PersonCertificate(item);
            if (dbresult.result == true)
            {
                new FileUploadSave(Request).SaveAs();
            }
            var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


        [PageInfo("Personel Sertifikası Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWSH_PersonCertificateById(id);
		    return View(data);
		}


        [PageInfo("Personel Sertifikası Güncelleme", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SH_PersonCertificate item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateSH_PersonCertificate(item);
            if (dbresult.result == true)
            {
                new FileUploadSave(Request).SaveAs();
            }
            var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        [PageInfo("Personel Sertifikası Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new SH_PersonCertificate { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSH_PersonCertificate(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        
        [PageInfo("Personel Sertifikası Grup İsimleri", SHRoles.IKYonetici)]
        public ContentResult CertificateNameGroup()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_PersonCertificate().GroupBy(x => x.CertificateName);
            var veri = data.Where(x => x.Key != null).Select(c => c.Key).ToArray();
            return Content(Infoline.Helper.Json.Serialize(veri), "application/json");
        }
    }
}