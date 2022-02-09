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

        [PageInfo("Personel giriş çıkış bilgilerini detaylı bir şekilde listeleyen sayfadır", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ActionResult TotalStaffWorkingStatus(string id, string date)
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


        [PageInfo("Personelin Çalışma Süresinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici)]
        public ContentResult GetDataPersonTotalWorking(DateTime date, Guid? userId)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var res = new VMShiftTrackingModel().GetDataPersonTotalWorking(date, userStatus.user.id);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).FirstOrDefault(a =>a.userId == userStatus.user.id).totalWorking), "application/json");
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

        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Toplamının Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult GetGeneralDataReportResultTotal(DateTime startDate, DateTime endDate, List<Guid> userIds)
        {
            var res = new VMShiftTrackingModel().GetGeneralDataReportResultTotal(startDate, endDate, userIds);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }

        [PageInfo("Mesai Başla", SHRoles.Personel)]
        [HttpPost]
        public JsonResult WorkStart(SH_ShiftTracking item)     
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];

            var shift = db.GetVWSH_ShiftTracking().Where(a => a.userId == userStatus.user.id).OrderByDescending(a => a.created).FirstOrDefault().shiftTrackingStatus;

            if(shift != 1 && shift != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = item.id,
                    FeedBack = feedback.Warning("Mesaiye Başlanma Başarısız.")
                }, JsonRequestBehavior.AllowGet);
            }
         
            item.id = Guid.NewGuid();
            item.shiftTrackingStatus = 0;
            item.userId = userStatus.user.id;
            item.timestamp = DateTime.Now;
            var dbresult = db.InsertSH_ShiftTracking(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Mesaiye Başlandı.") : feedback.Warning("Mesaiye Başlanma Başarısız.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Mola Ver", SHRoles.Personel)]
        [HttpPost]
        public JsonResult StartBreak(SH_ShiftTracking item)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];

            var shift = db.GetVWSH_ShiftTracking().Where(a => a.userId == userStatus.user.id).OrderByDescending(a => a.created).FirstOrDefault().shiftTrackingStatus;
            if (shift != 0 && shift != 3)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = item.id,
                    FeedBack = feedback.Warning("Mola Verme Başarısız")
                }, JsonRequestBehavior.AllowGet);
            }
            item.id = Guid.NewGuid();
            item.shiftTrackingStatus = 2;
            item.userId = userStatus.user.id;
            item.timestamp = DateTime.Now;
            var dbresult = db.InsertSH_ShiftTracking(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Mola Verildi.") : feedback.Warning("Mola Verme Başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Molayı Bitir", SHRoles.Personel)]
        [HttpPost]
        public JsonResult FinishTheBreak(SH_ShiftTracking item)

        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];

            var shift = db.GetVWSH_ShiftTracking().Where(a => a.userId == userStatus.user.id).OrderByDescending(a => a.created).FirstOrDefault().shiftTrackingStatus;
            if (shift != 2)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = item.id,
                    FeedBack = feedback.Warning("Mola Bitirilme İşlemi Başarısız Oldu.")
                }, JsonRequestBehavior.AllowGet);
            }
            item.id = Guid.NewGuid();
            item.shiftTrackingStatus = 3;
            item.userId = userStatus.user.id;
            item.timestamp = DateTime.Now;
            var dbresult = db.InsertSH_ShiftTracking(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Mola Bitirildi.") : feedback.Warning("Mola Bitirilme İşlemi Başarısız Oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Mesai Bitir", SHRoles.Personel)]
        [HttpPost]
        public JsonResult FinishTheWork(SH_ShiftTracking item)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];

            var shift = db.GetVWSH_ShiftTracking().Where(a => a.userId == userStatus.user.id).OrderByDescending(a => a.created).FirstOrDefault().shiftTrackingStatus;
            if (shift != 3 && shift != 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = item.id,
                    FeedBack = feedback.Warning("Mesaiyi Bitirilme Başarısız Oldu.")
                }, JsonRequestBehavior.AllowGet);
            }
            item.id = Guid.NewGuid();
            item.shiftTrackingStatus = 1;
            item.userId = userStatus.user.id;
            item.timestamp = DateTime.Now;
            var dbresult = db.InsertSH_ShiftTracking(item);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Mesai Bitirildi.") : feedback.Warning("Mesaiyi Bitirilme Başarısız Oldu.")
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
