using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.INV.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Infoline.WorkOfTime.WebProject.UIHelper.Utility;
namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
    public class VWINV_CommissionsController : Controller
    {
        [PageInfo("Tüm Görevlendirmeler", SHRoles.IKYonetici, SHRoles.IdariPersonel)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Görevlendirme Taleplerim", SHRoles.Personel)]
        public ActionResult MyIndex()
        {
            return View();
        }
        [PageInfo("Görevlendirme Talepleri (Onay)", SHRoles.Personel, SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        public ActionResult MyAboutIndex()
        {
            return View();
        }
        [PageInfo("Personel Görevlendirme Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_Commissions(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CommissionsCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Personel Görevlendirme Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWINV_CommissionsCount(condition.Filter);
            return count;
        }
        [PageInfo("Personel Görevlendirme Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var commision = db.GetVWINV_CommissionsById(id);
            ViewBag.Projects = db.GetVWINV_CommissionsProjectsIds(commision.id) ?? new VWINV_CommissionsProjects[] { };
            ViewBag.Persons = db.GetVWINV_CommissionsPersonCommissionIds(commision.id) ?? new VWINV_CommissionsPersons[] { };
            ViewBag.Information = db.GetVWINV_CommissionsInformationCommissionId(commision.id) ?? new VWINV_CommissionsInformation { };
            return View(commision);
        }
        [PageInfo("Personel Görevlendirmesi Ekleme", SHRoles.Personel, SHRoles.ProjeYonetici)]
        public ActionResult Insert(Guid? projectId)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            ViewBag.ProjectsId = projectId;
            var now = DateTime.Now;
            var start = INV_CommissionsCalculator.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
            var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
            var end = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);
            var data = new VWINV_Commissions
            {
                id = Guid.NewGuid(),
                createdby = userStatus.user.id,
                createdby_Title = userStatus.user.firstname + " " + userStatus.user.lastname,
                created = DateTime.Now,
                CommissionCode = BusinessExtensions.B_GetIdCode(),
                Manager1Approval = userStatus.user.Manager1,
                Manager1Approval_Title = userStatus.user.Manager1_Title,
                StartDate = start,
                EndDate = end,
            };
            return View(data);
        }
        [PageInfo("Personel Görevlendirmesi Ekleme", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(INV_Commissions item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = new List<ResultStatus>();
            item.ApproveStatus = (Int32)EnumINV_CommissionsApproveStatus.Bekliyor;
            var totals = INV_CommissionsCalculator.Calculate(new INV_Commissions
            {
                StartDate = item.StartDate,
                EndDate = item.EndDate,
            });
            item.TotalDays = totals.TotalDay;
            item.TotalHours = totals.TotalHour;
            if (item.StartDate == item.EndDate)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Tarih aralığını kontrol edin.") }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Request["idUsers"]))
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Görevlendirme işlemi için kullanıcı seçmelisiniz..") }, JsonRequestBehavior.AllowGet);
            }
            if (item.TravelInformation == (Int32)EnumINV_CommissionsTravelInformation.Arac && !item.IdCompanyCar.HasValue)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Görevlendirmeyi araç ile gerçekleştirdiğinizden dolayı araç seçmelisiniz.") }, JsonRequestBehavior.AllowGet);
            }
            var personList = Request["IdUsers"].Split(',').Select(x => new INV_CommissionsPersons
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                IdCommisions = item.id,
                IdUser = x.ToGuid(),
                IsOwner = userStatus.user.id == x.ToGuid() ? Convert.ToBoolean((Int32)EnumINV_CommissionsPersonsIsOwner.Evet) : Convert.ToBoolean((Int32)EnumINV_CommissionsPersonsIsOwner.Hayir),
            });
            if (personList.Where(x => x.IsOwner == true).Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kendinizi seçmeden görevlendirme talep edemezsiniz.Lütfen kendinizi ekleyerek tekrar talep gerçekleştiriniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            var owner = personList.Where(a => a.IsOwner == true).Select(a => a.IdUser.Value).FirstOrDefault();
            var personelPosition = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(owner, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault();
            if (personelPosition != null && personelPosition.Manager1.HasValue)
            {
                item.Manager1Approval = personelPosition.Manager1;
            }
            else
            {
                item.ApproveStatus = (Int32)EnumINV_CommissionsApproveStatus.Onaylandi;
                item.Manager1ApprovalDate = DateTime.Now;
            }
            var personelsInfo = db.GetVWSH_UserByIds(personList.Select(x => x.IdUser.Value).ToArray());
            foreach (var person in personList)
            {
                var personName = personelsInfo.Where(a => a.id == person.IdUser.Value).FirstOrDefault();
                var permitControl = db.GetINV_PermitByControlDate(person.IdUser.Value, item.StartDate, item.EndDate);
                if (permitControl != null)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning(string.Format("{0} isimli personelin {1} - {2} tarihleri arasında izin talebi mevcuttur. !", personName.firstname + " " + personName.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", permitControl.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", permitControl.EndDate)))
                    }, JsonRequestBehavior.AllowGet);
                }
                var commissionControl = db.GetVWINV_CommissionsPersonsByControlDate(person.IdUser.Value, item.StartDate, item.EndDate);
                if (commissionControl != null)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning(string.Format("{0} isimli personelin {1} - {2} tarihleri arasında görev talebi mevcuttur. !", personName.firstname + " " + personName.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", commissionControl.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", commissionControl.EndDate)))
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            var projects = Request["IdProject"] ?? "";
            var projectList = projects.Split(',').Where(a => a.ToGuid().HasValue == true).Select(x => new INV_CommissionsProjects
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                IdCommissions = item.id,
                IdProject = x.ToGuid()
            }).ToArray();
            var yuzdelik = Math.Round(Convert.ToDouble(100) / Convert.ToDouble(projectList.Count()), 1);
            foreach (var tile in projectList)
            {
                tile.Percentile = yuzdelik;
            }
            var trans = db.BeginTransaction();
            dbresult.Add(db.InsertINV_Commissions(item, trans));
            dbresult.Add(db.BulkInsertINV_CommissionsPersons(personList, trans));
            dbresult.Add(db.BulkInsertINV_CommissionsProjects(projectList, trans));
            if (dbresult.Count(a => a.result == false) > 0)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Görev formu kaydetme işlemi başarısız. Lütfen Daha Sonra Tekrar Deneyiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            trans.Commit();
            var ownerInfo = personelsInfo.Where(a => a.id == owner).FirstOrDefault();
            var created = string.Format("{0:dd.MM.yyyy HH:mm}", item.created);
            var start = string.Format("{0:dd.MM.yyyy HH:mm}", item.StartDate);
            var end = string.Format("{0:dd.MM.yyyy HH:mm}", item.EndDate);
            var personels = string.Join(", ", personelsInfo.Select(x => x.firstname + " " + x.lastname).ToArray());
            var tenantName = TenantConfig.Tenant.TenantName;
            var notification = new BusinessAccess.Notification();
            if (item.Manager1Approval.HasValue)
            {
                var companyPerson = db.GetVWSH_UserById(item.Manager1Approval.Value);
                var notiftText = "Sayın {0}, {1}  isimli personel {2} tarihinde, {3} - {4} tarihleri arasında   ";
                if (personelsInfo.Count() > 1)
                {
                    notiftText += personels + " personelleri için ";
                }
                notiftText += "görevlendirme talebinde bulunmuştur.</p> ";
                var text = "<h3>Sayın {0},</h3>";
                text += "<p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında  ";
                if (personelsInfo.Count() > 1)
                {
                    text += personels + " personelleri için ";
                }
                text += "görevlendirme talebinde bulunmuştur.</p> ";
                var siteName = TenantConfig.Tenant.GetWebUrl();
                text += "Onaya gitmek için lütfen <a href='{5}/INV/VWINV_Commissions/Update?id={6}'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                var mesaj = string.Format(text, companyPerson.FullName, ownerInfo.FullName, created, start, end, siteName, item.id);
                var notify = String.Format(notiftText, companyPerson.FullName, ownerInfo.FullName, created, start, end); ;
                new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                               .Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, companyPerson.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Görevlendirme Onayı Hakkında.."), true);
                notification.NotificationSend(companyPerson.id, "Görev Talep Onayı Hakkında", notify);
            }
            else
            {
                foreach (var person in personelsInfo)
                {
                    var text = "<h3>Sayın {0},</h3>";
                    var notifyText = "Sayın {0}";
                    if (ownerInfo.id == person.id)
                    {
                        text += "<p>{1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi sistem tarafından otomatik onaylanmıştır.</p>";
                        notifyText += ", {1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi sistem tarafından otomatik onaylanmıştır.";
                    }
                    else
                    {
                        text += "<p> " + ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi sistem tarafından otomatik onaylanmıştır.</p>";
                        notifyText += ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi sistem tarafından otomatik onaylanmıştır.";
                    }
                    text += "Islak imzalı görevlendirme formunu yüklemek için lütfen <a href='{4}/INV/VWINV_Commissions/MyIndex'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                    var siteName = TenantConfig.Tenant.GetWebUrl();
                    var mesaj = string.Format(text, person.FullName, created, start, end, siteName);
                    var notify = string.Format(notifyText, person.FullName, created, start, end);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                              .Send((Int16)EmailSendTypes.GorevlendirmeSurecTamamlama, person.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Görevlendirme Onayı Hakkında.."), true);
                    notification.NotificationSend(person.id, "Görev Talep Onayı Hakkında", notify);
                }
            }
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Görev kaydetme işlemi başarı ile gerçekleşti", false, (Request.UrlReferrer.AbsolutePath == "/INV/VWINV_Commissions/Insert" ? Url.Action("MyIndex", "VWINV_Commissions", new { area = "INV" }) : null))
            }, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Personel Seyahat Bilgileri Ekleme", SHRoles.Personel, SHRoles.IdariPersonel)]
        public ActionResult TravelInsert(Guid? commissionsId, int? travelInformation, int? requestForAccommodation, DateTime? startDate, DateTime? endDate)
        {
            var db = new WorkOfTimeDatabase();
            var existComissions = db.GetVWINV_CommissionsInformationCommissionId(commissionsId.Value);
            var data = existComissions != null ? existComissions : new VWINV_CommissionsInformation();
            if (data == null || data.commissionsId == null)
            {
                data.commissionsId = commissionsId;
                data.travelInformation = travelInformation;
                data.requestForAccommodation = requestForAccommodation;
                data.departureDate = startDate;
                data.hotelEntryDate = startDate;
                data.rentalCarStartDate = startDate;
                data.shuttleDepartureDate = startDate;
                data.shuttleReturnDate = endDate;
                data.returnDate = endDate;
                data.rentalCarEndDate = endDate;
                data.hotelLeaveDate = endDate;
            }
            return View(data);
        }
        [PageInfo("Personel Seyahat Bilgileri Ekleme", SHRoles.Personel, SHRoles.IdariPersonel)]
        [HttpPost]
        public JsonResult TravelInsert(INV_CommissionsInformation item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var result = new List<ResultStatus>();
            if (item.commissionsId != null)
            {
                var existTravel = db.GetVWINV_CommissionsById(item.commissionsId.Value);
                var checkUser = db.GetINV_CommissionsPersonCommissionId(item.commissionsId.Value);
                var existTravelInfo = db.GetINV_CommissionsInformationById(item.id);
                foreach (var user in checkUser)
                {
                    if (existTravelInfo == null)
                    {
                        item.created = DateTime.Now;
                        item.createdby = userStatus.user.id;
                        item.userId = user.id;
                        if (Request != null)
                        {
                            new FileUploadSave(Request, item.id).SaveAs();
                        }
                        result.Add(db.InsertINV_CommissionsInformation(new INV_CommissionsInformation().B_EntityDataCopyForMaterial(item)));
                    }
                    else
                    {
                        item.userId = user.id;
                        item.changed = DateTime.Now;
                        item.changedby = userStatus.user.id;
                        if (Request != null)
                        {
                            new FileUploadSave(Request, item.id).SaveAs();
                        }
                        result.Add(db.UpdateINV_CommissionsInformation(new INV_CommissionsInformation().B_EntityDataCopyForMaterial(item), false));
                    }
                }
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kayıt Bulunamadı")
                }, JsonRequestBehavior.AllowGet);

            }

            if (result.Count(a => a.result == false) == 0)
            {
                var filesList = new List<string>();
                var files = db.GetSYS_FilesByDataIdArray(item.id);
                var webUrl = TenantConfig.Tenant.GetWebUrl();

                if (files.Count() > 0)
                {
                    foreach (var filesData in files)
                    {
                        filesList.Add(webUrl + "" + filesData.FilePath);
                    }
                }
               
                var checkUser = db.GetINV_CommissionsPersonCommissionId(item.commissionsId.Value);
                var url = TenantConfig.Tenant.GetWebUrl();
                var tenantName = TenantConfig.Tenant.TenantName;
                foreach (var user in checkUser)
                {
                    var users = db.GetVWSH_UserById(user.IdUser.Value);
                    var text = @"<h3>Sayın {0},</h3> 
                     <p>İdari Yöneticiniz, Seyahat Bilgileri Ve Dokümanlarınızı Sistemde Düzenlemiştir. </p>
                     <p>Kontrol etmek için lütfen <a href='{1}/INV/VWINV_Confirmation/Detail?id={2}'>Buraya tıklayınız! </a></p>
                     <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                    var mesaj = string.Format(text, users.FullName, url, item.commissionsId);
                    new Email().Template("Template1", "working.jpg", "Görevlendirme Seyahat Bilgileri Hakkında", mesaj)
                    .Send((Int16)EmailSendTypes.GorevlendirmeSurecTamamlama, users.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Seyahat Bilgileri Hakkında.."), true,null,null,filesList.ToArray());
                }

                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Seyahat Bilgileri Güncelleme Başarılı", false, Url.Action("Detail", "VWINV_Commissions", new { area = "INV", id = item.commissionsId }))
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Seyahat Bilgileri Güncelleme Başarısız.")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [PageInfo("Personel Görevlendirmesi Güncelleme", SHRoles.Personel, SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var commision = db.GetVWINV_CommissionsById(id);
            if (commision == null)
            {
                new FeedBack().Warning("Görevlendirme İptal Edildiği İçin Görevlendirmeler Sayfasına Yönlendiriliyorsunuz.", true);
                return RedirectToAction("Index", "VWINV_Commissions", new { area = "INV" });
            }
            ViewBag.Projects = db.GetVWINV_CommissionsProjectsIds(commision.id) ?? new VWINV_CommissionsProjects[] { };
            ViewBag.Persons = db.GetVWINV_CommissionsPersonCommissionIds(commision.id) ?? new VWINV_CommissionsPersons[] { };
            return View(commision);
        }
        [PageInfo("Personel Görevlendirmesi Güncelleme", SHRoles.Personel, SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(INV_Commissions item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var tenantName = TenantConfig.Tenant.TenantName;
            var commision = db.GetVWINV_CommissionsById(item.id);
            if (commision == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Görevlendirme bulunamadı.Silinmiş olabilir.")
                }, JsonRequestBehavior.AllowGet);
            }
            commision.changed = DateTime.Now;
            commision.changedby = userStatus.user.id;
            var persons = db.GetVWINV_CommissionsPersonCommissionIds(item.id);
            var notification = new BusinessAccess.Notification();
            //Yönetici Onayı
            if (userStatus.user.id == commision.Manager1Approval && commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor)
            {
                commision.Manager1ApprovalDate = DateTime.Now;
                commision.ApproveStatus = item.ApproveStatus;
                commision.ApproveStatus_Title = ((EnumINV_CommissionsApproveStatus)commision.ApproveStatus).ToDescription();
                var allinsankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
                foreach (var insankaynaklari in allinsankaynaklari)
                {
                    var textReq = "";
                    if (commision.RequestForAccommodation == (int)EnumINV_CommissionsRequestForAccommodation.Var)
                    {
                        textReq += commision.RequestForAccommodation == (int)EnumINV_CommissionsRequestForAccommodation.Var ?
                               "Konaklama ve Avans " :
                               commision.RequestForAccommodation == (int)EnumINV_CommissionsRequestForAccommodation.Var ?
                               "Konaklama " : "";
                    }


                    var text = "<h3>Sayın İnsan Kaynakları Yöneticisi,</h3>";
                    text += "<p>{0} - {1} tarihleri arasında  {2} adlı personel(ler) görevlendirme gerçekleştirecektir.</p>";
                    text += "<p>Seyahati gerçekleştireceği adres : {3} </p>";
                    if (textReq != "")
                    {
                        text += "<p>Bilgilendirme : {5} talebi bulunmaktadır. Talep Tutarı : {6}</p>";
                    }
                    text += "<p>Bilgilerinize.</p>";
                    var asUser = persons.Where(x => x.IdUser != null).Select(x => x.IdUser.Value).ToArray();
                    var userMail = db.GetVWSH_UserByIds(asUser);
                    var userss = string.Join(", ", userMail.Select(x => x.FullName));
                    var siteName = TenantConfig.Tenant.GetWebUrl();
                    var mesaj = string.Format(text,
                        string.Format("{0:dd.MM.yyyy HH:mm}", commision.StartDate),
                        string.Format("{0:dd.MM.yyyy HH:mm}", commision.EndDate),
                        userss,
                        string.IsNullOrEmpty(commision.ToAdress) ? "Belirtilmemiş" : commision.ToAdress,
                        siteName,
                        textReq,
                        "");
                    var notify = String.Format("Sayın İnsan Kaynakları Yöneticisi, {0} - {1} tarihleri arasında  {2} adlı personel(ler) görevlendirme gerçekleştirecektir.", string.Format("{0:dd.MM.yyyy HH:mm}", commision.StartDate),
                       string.Format("{0:dd.MM.yyyy HH:mm}", commision.EndDate),
                       userss);
                    notification.NotificationSend(insankaynaklari.id, "Görevlendirmeye Çıkacak Personeller Hakkında", notify);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görevlendirmeye Çıkacak Personeller Hakkında", mesaj).Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Görevlendirmeye Çıkacak Personeller Hakkında"), true);
                }
                if (commision.RequestForAccommodation == (int)EnumINV_CommissionsRequestForAccommodation.Var)
                {
                    var textAll = commision.RequestForAccommodation == (int)EnumINV_CommissionsRequestForAccommodation.Var ?
                        "Konaklama ve Avans " :
                        commision.RequestForAccommodation == (int)EnumINV_CommissionsRequestForAccommodation.Var ?
                        "Konaklama " : "";
                }
            }
            //Islak İmza Yükleme
            if (persons.Count(a => a.IdUser == userStatus.user.id && string.IsNullOrEmpty(a.Files)) > 0 && commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi)
            {
                var rsFile = new FileUploadSave(Request).SaveAs();
                persons = db.GetVWINV_CommissionsPersonCommissionIds(item.id);
                if (persons.Count(a => string.IsNullOrEmpty(a.Files)) == 0)
                {
                    commision.ApproveStatus = (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi;
                    commision.ApproveStatus_Title = ((EnumINV_CommissionsApproveStatus)commision.ApproveStatus).ToDescription();
                }
            }
            var dbresult = db.UpdateINV_Commissions(new INV_Commissions().EntityDataCopyForMaterial(commision, true));
            if (!dbresult.result)
            {
                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = feedback.Warning("Görev yanıtlandırma işlemi başarısız.Lütfen sistem yöneticinizle görüşünüz.")
                }, JsonRequestBehavior.AllowGet);
            }
            var personsInfo = db.GetVWSH_UserByIds(persons.Select(a => a.IdUser.Value).ToArray());
            var owner = persons.Where(a => a.IsOwner == true).FirstOrDefault();
            var ownerInfo = personsInfo.Where(a => a.id == owner.IdUser).FirstOrDefault();
            var created = string.Format("{0:dd.MM.yyyy HH:mm}", item.created);
            var start = string.Format("{0:dd.MM.yyyy HH:mm}", item.StartDate);
            var end = string.Format("{0:dd.MM.yyyy HH:mm}", item.EndDate);
            var url = TenantConfig.Tenant.GetWebUrl();
            if (ownerInfo == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = feedback.Warning("Görev talebini yapan personel bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }
            if (commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi && userStatus.user.id == commision.Manager1Approval)
            {
                var personels = String.Join(", ", personsInfo.Select(a => a.FullName).ToArray());
                var text = "<h3>Sayın {0},</h3>";
                text += "<p>{1} tarihinde, {2} - {3} tarihleri arasında oluşturduğunuz görevlendirme talebi yöneticiniz {5} tarafından reddedildi.</p>";
                text += "<p>Bilgilerinize.</p>";
                var mesaj = string.Format(text, ownerInfo.FullName, created, start, end, personels, commision.Manager1Approval_Title);

                var notify = string.Format("Sayın {0}, {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğunuz görevlendirme talebi yöneticiniz {5} tarafından reddedildi. ", ownerInfo.FullName, created, start, end, personels, commision.Manager1Approval_Title);
                notification.NotificationSend(ownerInfo.id, "Görev Talep Onayı Hakkında", notify);
                new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                        .Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, ownerInfo.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Görevlendirme Onayı Hakkında.."), true);

            }
            if (commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi && userStatus.user.id == commision.Manager1Approval)
            {
                foreach (var person in personsInfo)
                {
                    var text = "<h3>Sayın {0},</h3>";
                    var notifyText = "Sayın {0}";
                    if (ownerInfo.id == person.id)
                    {
                        notifyText += "{1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi onaylanmıştır.";
                        text += "<p>{1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi onaylanmıştır.</p>";
                    }
                    else
                    {
                        notifyText += ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi onaylanmıştır.";
                        text += "<p> " + ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi onaylanmıştır.</p>";
                    }
                    text += "Islak imzalı görevlendirme formunu yüklemek için lütfen <a href='{4}/INV/VWINV_Commissions/MyIndex'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                    var siteName = TenantConfig.Tenant.GetWebUrl();
                    var notify = string.Format(notifyText, person.FullName, created, start, end);
                    var mesaj = string.Format(text, person.FullName, created, start, end, siteName);
                    notification.NotificationSend(person.id, "Görev Talep Onayı Hakkında", notify);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                       .Send((Int16)EmailSendTypes.GorevlendirmeSurecTamamlama, person.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Görevlendirme Onayı Hakkında.."), true);

                }
                var IdariPersonelYonetici = db.GetSH_UserByRoleId(SHRoles.IdariPersonelYonetici).FirstOrDefault();
                var travelInfo = item.TravelInformation == 0 ? "Şirket Aracı" : item.TravelInformation == 1 ? "Otobüs" : item.TravelInformation == 2 ? "Uçak" : item.TravelInformation == 4 ? "Kiralık Araç + Uçak" : item.TravelInformation == 5 ? "Shuttle" : null;
                if (item.TravelInformation == 3)
                {
                    travelInfo = item.TravelInformationDetail;
                }
                if (IdariPersonelYonetici != null && ownerInfo != null)
                {
                    var baslik = "<h3>Sayın {0},</h3>";
                    baslik += "<p>{1} tarihinde, {2} - {3} tarihleri arasında {4} isimli personelin görevlendirme talebi onaylanmıştır.</p>";
                    baslik += "<p>Seyahat Aracı : {6}</p>";
                    baslik += "Görevlendirme formunu görüntülemek için lütfen <a href='{5}/INV/VWINV_Commissions/Index'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                    var siteName = TenantConfig.Tenant.GetWebUrl();
                    var mesajText = string.Format(baslik, IdariPersonelYonetici.firstname + " " + IdariPersonelYonetici.lastname, created, start, end, ownerInfo.FullName, siteName, travelInfo);
                    var nofity = string.Format("Sayın {0}, {1} tarihinde, {2} {3} tarihleri arasında {4} isimli personelin görevlendirme talebi onaylanmıştır.", IdariPersonelYonetici.firstname + " " + IdariPersonelYonetici.lastname, created, start, end, ownerInfo.FullName);
                    notification.NotificationSend(IdariPersonelYonetici.id, "Görev Talebi Bilgilendirmesi Hakkında", nofity);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talebi Bilgilendirmesi Hakkında", mesajText)
                      .Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, IdariPersonelYonetici.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Görev Talebi Bilgilendirmesi Hakkında.."), true);
                }
            }
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = feedback.Success("Görev yanıtlama işlemi başarılı", false, Url.Action((userStatus.user.id == commision.Manager1Approval ? "MyAboutIndex" : "MyIndex"), "VWINV_Commissions"))
            }, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Personel Görevlendirmesi Silme", SHRoles.Personel, SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["UserStatus"];
            var dbresult = new List<ResultStatus>();
            var item = db.GetINV_CommissionsById(id);
            if (item == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Silmeye çalışılan görevlendirme bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }
            var itemUser = db.GetVWINV_CommissionsPersons().Where(c => c.id == item.id).ToArray();
            var temUser = db.GetINV_CommissionsPersons().Where(c => c.IdUser == userStatus.user.id && c.IsOwner != true && id == c.IdCommisions).ToArray();
            if (temUser.Count() > 0)
            {
                var trans = db.BeginTransaction();
                dbresult.Add(db.BulkDeleteINV_CommissionsPersons(temUser, trans));
                if (dbresult.Count(a => a.result == false) > 0)
                {
                    trans.Rollback();
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning("Silme İşlemi Başarısız.")
                    }, JsonRequestBehavior.AllowGet);
                }
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Bulunduğunuz görevlendirme talebinden kendinizi sildiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            var user = itemUser.Where(c => c.IsOwner == true).Select(x => x.IdUser.Value);
            var itemProject = db.GetINV_CommissionsProjectCommissionId(item.id).ToArray();
            var asitemUser = db.GetINV_CommissionsPersons().Where(c => c.IdCommisions == itemUser.Select(v => v.id).FirstOrDefault()).ToArray();
            if (userStatus.user.id == user.FirstOrDefault())
            {
                if (item.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor)
                {
                    var trans = db.BeginTransaction();
                    dbresult.Add(db.BulkDeleteINV_CommissionsProjects(itemProject, trans));
                    dbresult.Add(db.BulkDeleteINV_CommissionsPersons(asitemUser, trans));
                    dbresult.Add(db.DeleteINV_Commissions(item, trans));
                    if (dbresult.Count(a => a.result == false) > 0)
                    {
                        trans.Rollback();
                        return Json(new ResultStatusUI
                        {
                            Result = false,
                            FeedBack = feedback.Warning("Silme İşlemi Başarısız.")
                        }, JsonRequestBehavior.AllowGet);
                    }
                    trans.Commit();
                    return Json(new ResultStatusUI
                    {
                        Result = true,
                        FeedBack = feedback.Success("Talep etmiş olduğunuz görevlendirme talebinizi sildiniz.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.IKYonetici)))
            {
                var trans = db.BeginTransaction();
                dbresult.Add(db.BulkDeleteINV_CommissionsProjects(itemProject, trans));
                dbresult.Add(db.BulkDeleteINV_CommissionsPersons(asitemUser, trans));
                dbresult.Add(db.DeleteINV_Commissions(item, trans));
                if (dbresult.Count(a => a.result == false) > 0)
                {
                    trans.Rollback();
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning("Silme İşlemi Başarısız.")
                    }, JsonRequestBehavior.AllowGet);
                }
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Görevlendirme talebini silme işlemi gerçekleşti.")
                }, JsonRequestBehavior.AllowGet);
            }
            var result = new ResultStatusUI
            {
                Result = false,
                FeedBack = feedback.Warning("Silme işlemi başarısız oldu.")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Görevlendirme Talebi Form Sayfası", SHRoles.Personel)]
        public ActionResult CommissionsForm(Guid id)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CommissionsById(id);
            var userStatus = (PageSecurity)Session["userStatus"];
            if (data == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Personel Bilgilerine Ulaşılamadı")
                }, JsonRequestBehavior.AllowGet);
            }
            var persons = db.GetVWINV_CommissionsPersonCommissionIds(data.id);
            if (persons == null || persons.Count(a => a.IdUser == userStatus.user.id) == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Görevlendirme formunu sadece görevlendirmeye dahil kullanıcılar görüntüleyebilir.")
                }, JsonRequestBehavior.AllowGet);
            }
            var siteName = TenantConfig.Tenant.GetWebUrl();
            var file = "";
            var company = db.GetVWINV_CompanyPersonLastByUserId(persons.Select(c => c.IdUser.Value).FirstOrDefault());
            if (company != null)
            {
                var sysLogo = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("CMP_Company", "İşletme Logosu", company.CompanyId.Value);
                file = sysLogo != null ? sysLogo.FilePath : "";
            }
            var model = new VWINV_CommissionsForm
            {
                Projects = db.GetVWINV_CommissionsProjectsIds(data.id),
                Users = persons,
                CurrentUser = userStatus.user,
                LogoPath = file,
                Url = siteName + "/INV/VWINV_Permit/Detail?id=" + data.id,
                CommissionTime = INV_CommissionsCalculator.Calculate(new INV_Commissions() { StartDate = data.StartDate, EndDate = data.EndDate }).Text
            }.EntityDataCopyForMaterial(data, true);
            var datadepartman = db.GetVWINV_CompanyPersonDepartmentsByIdUserAllDepartmentsOfUser(model.CurrentUser.id).Where(x => x.EndDate == null);
            model.Departman = datadepartman.Select(x => x.PID_Title).FirstOrDefault();
            model.Position = datadepartman.Select(x => x.Title).FirstOrDefault();
            return View(model);
        }
        [PageInfo("Görevlendirme Detay Bilgileri", SHRoles.Personel)]
        public JsonResult GetCommissionsDetails(INV_Commissions item)
        {
            return Json(INV_CommissionsCalculator.Calculate(item), JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Görevlendirme Raporları", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        public ActionResult Dashboard()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var dateFunc = new DateFunctions();
            var commissions = db.GetVWINV_CommissionsByYear(dateFunc.BuYilBasi);
            var commisPerson = db.GetVWINV_CommissionsPersonsByYear(dateFunc.BuYilBasi);
            var pageReport = new VWINV_CommissionsPageReport
            {
                idUser = userStatus.user.id,
                BuYilGorevlendirilmeSayisi = commissions.Count(),
                BuAyGorevlendirilmeSayisi = commissions.Where(x => x.StartDate >= dateFunc.AyBasi && x.StartDate <= dateFunc.AySonu).Count(),
                BuHaftaGorevlendirilmeSayisi = commissions.Where(x => x.StartDate >= dateFunc.HaftaBasi && x.StartDate <= dateFunc.BuHaftaSonu).Count(),
                BuYilEnCokGorevlendirilenPersonel = commisPerson.Where(c => c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).GroupBy(x => x.Person_Title).Select(x => new { Count = x.Count(), Personel = x.Select(b => b.Person_Title) }).OrderByDescending(x => x.Count).FirstOrDefault()?.Personel.FirstOrDefault(),
                EnFazlaGorevlendirilmeTipi = commissions.GroupBy(x => x.Information_Title).Select(x => new { Count = x.Count(), Tip = x.Select(b => b.Information_Title) }).OrderByDescending(x => x.Count).FirstOrDefault()?.Tip.FirstOrDefault()
            };
            return View(pageReport);
        }
        [PageInfo("Görevlendirme Raporu Grafikleri", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        public JsonResult DashboardResult()
        {
            try
            {
                var DashboardModel = new INV_CommissionsDashboardModel();
                return Json(DashboardModel.GetResults(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = new FeedBack().Error(ex.Message, "Sonuç Yüklenemedi") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
