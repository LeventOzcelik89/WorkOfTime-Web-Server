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

        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici)]
        public ContentResult GetDataReportResult(DateTime date, Guid? userId)
        {
            var res = new VMShiftTrackingModel().GetDataReportResult(date, userId);
            return Content(Infoline.Helper.Json.Serialize(res.OrderByDescending(a => a.totalWorking).ToList()), "application/json");
        }
        [PageInfo("Personellerin Tüm Giriş Çıkış Verilerinin Dönüldüğü Methoddur.", SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
        public ContentResult GetDateDataReportResult(DateTime startDate, DateTime endDate, Guid? userId)
        {
            var db = new WorkOfTimeDatabase();
            var ourPersons = new List<VWSH_User>();
            if (userId.HasValue)
            {
                ourPersons.Add(db.GetVWSH_UserById(userId.Value));
            }
            else
            {
                ourPersons = db.GetVWSH_UserMyPerson().ToList();
            }

            var shiftTrackings = db.VWGetSH_ShiftTrackingByStartAndEndDate(startDate, endDate);
            var listModel = new List<VMSH_ShiftTrackingReport>();
            var listData = new List<VMSH_ShiftTrackingReport>();

            if (shiftTrackings.Count() > 0)
            {

                listModel.AddRange(ourPersons.Select(x => new VMSH_ShiftTrackingReport
                {
                    userId = ourPersons.FirstOrDefault()?.id,
                    startDate = startDate,
                    endDate = endDate,
                    CompanyId_Title = x.Company_Title,
                    UserId_Title = ourPersons.FirstOrDefault()?.FullName
                }));

                while (startDate <= endDate)
                {
                    foreach (var shiftTracking in listModel.ToList())
                    {
                        var workingMinutes = GetCalculateDateDayShift(shiftTracking.userId.Value, startDate, startDate.AddDays(1).AddMilliseconds(-1),
                            shiftTrackings.Where(a => a.userId == shiftTracking.userId && a.timestamp >= startDate && a.timestamp < startDate.AddDays(1).AddMilliseconds(-1)).ToArray());

                        TimeSpan ts = TimeSpan.FromMinutes(workingMinutes);
                        var workingHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                        listData.Add(new VMSH_ShiftTrackingReport
                        {
                            totalWorking = workingHoursStringValue.ToString(),
                            CompanyId_Title = shiftTracking.CompanyId_Title,
                            UserId_Title = shiftTracking.UserId_Title,
                            startDate = startDate,
                            endDate = endDate,
                            date= startDate,
                            userId = shiftTracking.userId
                        });
                    }
                    startDate=startDate.AddDays(1);
                }

            }
            return Content(Infoline.Helper.Json.Serialize(listData.OrderBy(a => a.date).ToList()), "application/json");
        }
       
        public double GetCalculateDateDayShift(Guid userId, DateTime shiftstart, DateTime shiftend, VWSH_ShiftTracking[] shiftTrackingReport)
        {
            if (shiftTrackingReport == null || shiftTrackingReport.Count() < 1)
            {


                var db = new WorkOfTimeDatabase();
                var lastAction = db.GetSH_ShiftTrackingFirstByUseridBeforeDate(userId, shiftstart);
                if (lastAction != null)
                {
                    if (lastAction.shiftTrackingStatus != (int)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti && lastAction.shiftTrackingStatus != (int)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti || lastAction.shiftTrackingStatus == (int)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi)
                    {
                        return 60 * 24;
                    }
                    else
                    {
                        return 0;
                    }
                }

                return 0;
            }
            else
            {
                var shiftMinutesList = new List<double>();
                var shiftTrackingReportList = shiftTrackingReport.OrderBy(a => a.timestamp).ToList();

                while (shiftTrackingReportList.Count() > 0)
                {
                    DateTime firstStartDate;
                    DateTime firstFinishDate;

                    var firstValue = shiftTrackingReportList.FirstOrDefault().shiftTrackingStatus;
                    if (firstValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi || firstValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti)
                    {
                        firstStartDate = shiftTrackingReportList.FirstOrDefault().timestamp.Value;
                        shiftTrackingReportList.Remove(shiftTrackingReportList.FirstOrDefault());

                        foreach (var item in shiftTrackingReportList.ToList())
                        {
                            if (item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi || item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti)
                                shiftTrackingReportList.Remove(item);
                            else
                                break;
                        }
                    }
                    else
                    {
                        firstStartDate = new DateTime(shiftstart.Year, shiftstart.Month, shiftstart.Day, 00, 00, 000);
                    }


                    if (shiftTrackingReportList.Count() > 0)
                    {
                        var firstFinishValue = shiftTrackingReportList.FirstOrDefault().shiftTrackingStatus;
                        if (firstFinishValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti || firstFinishValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                        {
                            firstFinishDate = shiftTrackingReportList.FirstOrDefault().timestamp.Value;
                            shiftTrackingReportList.Remove(shiftTrackingReportList.FirstOrDefault());

                            foreach (var item in shiftTrackingReportList.ToList())
                            {
                                if (item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti || item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                                    shiftTrackingReportList.Remove(item);
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (shiftstart.Year == DateTime.Now.Year && shiftstart.Month == DateTime.Now.Month && shiftstart.Day == DateTime.Now.Day)
                                firstFinishDate = DateTime.Now;
                            else
                                firstFinishDate = new DateTime(shiftstart.Year, shiftstart.Month, shiftstart.Day, 23, 59, 59);
                        }
                    }
                    else
                    {
                        if (shiftstart.Year == DateTime.Now.Year && shiftstart.Month == DateTime.Now.Month && shiftstart.Day == DateTime.Now.Day)
                            firstFinishDate = DateTime.Now;
                        else
                            firstFinishDate = new DateTime(shiftstart.Year, shiftstart.Month, shiftstart.Day, 23, 59, 59);
                    }

                    shiftMinutesList.Add((firstFinishDate - firstStartDate).TotalMinutes);
                }
                var totalMinutes = shiftMinutesList.Where(a => a > 0).Sum();
                return totalMinutes;
            }
        }
    }
}
