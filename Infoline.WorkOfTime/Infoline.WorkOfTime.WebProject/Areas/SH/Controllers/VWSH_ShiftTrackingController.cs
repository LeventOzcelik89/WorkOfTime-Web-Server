using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_ShiftTrackingController : Controller
    {
        [PageInfo("Personel giriş çıkış bilgileri veri methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_ShiftTracking(condition).ToDataSourceResult(request);
            data.Total = db.GetVWSH_ShiftTrackingCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Personel giriş çıkış bilgileri dropdown veri methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_ShiftTracking(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Personel giriş çıkış bilgileri detayı", SHRoles.IdariPersonelYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_ShiftTrackingById(id);
            return View(data);
        }

        [PageInfo("Personel Giriş Çıkış Log Ekleme", SHRoles.IdariPersonelYonetici)]

        public ActionResult Insert(Guid userId, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_ShiftTrackingByUserId(userId);

            return View(data);
        }

        [PageInfo("Personel Giriş Çıkış Log Ekleme", SHRoles.IdariPersonelYonetici)]
        [HttpPost]
        public JsonResult Insert(SH_ShiftTracking item)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            item.id = Guid.NewGuid();
            var dbresult = db.InsertSH_ShiftTracking(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Kayıt İşlemi Başarılı.") : feedback.Warning("Kayıt İşlemi Başarısız.")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Personel Giriş Çıkış Log Güncelleme", SHRoles.IdariPersonelYonetici)]

        public ActionResult Update(SH_ShiftTracking item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_ShiftTrackingById(item.id);
            return View(data);
        }

        [PageInfo("Personel Giriş Çıkış Log Güncelleme", SHRoles.IdariPersonelYonetici)]
        [HttpPost]
        public JsonResult Update(SH_ShiftTracking item)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbresult = db.UpdateSH_ShiftTracking(item, true);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Kayıt İşlemi Başarılı.") : feedback.Warning("Kayıt İşlemi Başarısız.")
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Personel Giriş Çıkış Log Silme İşlemi", SHRoles.IdariPersonelYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var data = db.GetSH_ShiftTrackingById(id);
            var dbresult = db.DeleteSH_ShiftTracking(data);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme İşlemi Başarılı") : feedback.Warning("Silme İşlemi Başarısız.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel Giriş-Çıkış Raporları", SHRoles.IdariPersonelYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [AllowEveryone]
        [PageInfo("Personel bazlı giriş çıkış raporları sayfası", SHRoles.IdariPersonelYonetici)]
        public ActionResult IndexReport()
        {
            return View();
        }

        [PageInfo("Personel giriş çıkış bilgilerini listeleyen sayfadır", SHRoles.IdariPersonelYonetici)]
        public ActionResult LogDetail(string id, string date)
        {

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(date))
            {
                ShiftTracking model = new ShiftTracking
                {
                    userId = Guid.Parse(id),
                    date = Convert.ToDateTime(date)

                };

                return View(model);
            }

            return View();
        }
        [PageInfo("Personel giriş çıkış bilgilerini detaylı bir şekilde listeleyen sayfadır", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ActionResult StaffWorkingStatus(string id, string date)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(date))
            {
                ShiftTracking model = new ShiftTracking
                {
                    userId = Guid.Parse(id),
                    date = Convert.ToDateTime(date)

                };

                return View(model);
            }
            return View();
        }

        

        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici)]
        public ContentResult GetDataReportResult(DateTime date, Guid? userId)
        {
            var res = new VMShiftTrackingModel().GetDataReportResult(date, userId);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }

        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult GetCalculateDateDayShift(DateTime date, Guid? userId)
        {
            var res = new VMShiftTrackingModel().GetDataReportResult(date, userId);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }

        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult GetDateDataReportResult(DateTime startDate, DateTime endDate, Guid? userId)
        {
            var res = new VMShiftTrackingModel().GetDateDataReportResult(startDate, endDate, userId);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }
        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult GetGeneralDataReportResult(DateTime startDate, DateTime endDate, List<Guid> userIds)
        {
            var res = new VMShiftTrackingModel().GetGeneralDataReportResult(startDate, endDate, userIds);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Toplamının Dönüldüğü Methoddur.")]
        public ContentResult GetGeneralDataReportResultTotal(DateTime startDate, DateTime endDate, List<Guid> userIds)
        {
            var res = new VMShiftTrackingModel().GetGeneralDataReportResultTotal(startDate, endDate, userIds);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }

    }
}
