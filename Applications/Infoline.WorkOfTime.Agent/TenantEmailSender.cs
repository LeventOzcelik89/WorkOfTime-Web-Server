using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Infoline.WorkOfTime.Agent
{
    public class TenantEmailSender : IMail, IDisposable
    {
        public TEN_Tenant _tenant { get; set; }
        public WorkOfTimeDatabase _db { get; set; }
        public string _url { get; set; }
        public Email _email { get; set; }
        bool start = false;
        private DateTime? _lastExecute;
        private PRJ_ProjectTimeline[] _timeline;
        private INV_CompanyPersonCalendar[] _events;
        private VWSH_User[] _persons;

        public TenantEmailSender(TEN_Tenant tenant)
        {
            _tenant = tenant;
            _db = _tenant.GetDatabase();
            _url = _tenant.GetWebUrl();
            _email = _tenant.GetEmailClass();
            Log.Success("Ajan Başlatıldı. ({0} {1})", _tenant.TenantName, _tenant.TenantCode);
            start = true;
            ProcessLoop();
        }

        public void ProcessLoop()
        {
            while (start)
            {

                var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

                try
                {

                    if (_lastExecute == null || _lastExecute.Value.Day != now.Day)
                    {
                        _persons = _db.GetVWSH_UserMyPerson();
                        Log.Info("Personel bilgileri Çekildi. ({0} {1})", _tenant.TenantName, _tenant.TenantCode);
                    }

                    if (_lastExecute == null || _lastExecute.Value.Hour != now.Hour)
                    {
                        _timeline = _db.GetPRJ_ProjectTimelineScheduledControl();
                        _events = _db.GetINV_CompanyPersonCalendarScheduled();
                        _lastExecute = now;
                        Log.Info("Takvim bilgileri çekildi. ({0} {1})", _tenant.TenantName, _tenant.TenantCode);
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("Personel bilgileri çekilirken sorunlar oluştu. Mesaj : {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
                }



                if (now.Hour == 8 && now.Minute == 30)
                {
                    Log.Info("Günlük işlem kontrolü başladı. ({0} {1})", _tenant.TenantName, _tenant.TenantCode);
                    PersonAssesment();
                    ProjectTimeline();
                    PersonBirthday();
                    TenderControl();
                    //PerDayMailService();
                    PersonWorkingStart();
                    Log.Info("Günlük işlem kontrolü bitti. ({0} {1})", _tenant.TenantName, _tenant.TenantCode);
                }

                if (now.Hour == 18 && now.Minute == 30)
                {
                    //EndOfDayMailService();
                    //Log.Info("Gün sonu maili gönnderildi.({0} {1})", _tenant.TenantName, _tenant.TenantCode);
                }

                if (now.DayOfWeek == DayOfWeek.Friday && now.Hour == 18 && now.Minute == 30)
                {
                    //WeeklyMailService();
                    //Log.Info("Haftalık mail gönnderildi.({0} {1})", _tenant.TenantName, _tenant.TenantCode);
                }

                PersonCalendar();

                new TaskPlanner(_tenant).CreateTasks(now);

                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
        }

        private void SendEmail(IEnumerable<SH_User> users, string title, string mailTemplate, params object[] mailTemplateParametre)
        {
            var html = string.Format(mailTemplate, mailTemplateParametre);


            Log.Info("{0} adreslerine '{1}' konulu mail yollanmıştır. ({2} {3})", string.Join(",", users.Select(a => a.email).ToArray()), title, _tenant.TenantName, _tenant.TenantCode);

            foreach (var email in users)
            {
                _email.Send((Int16)EmailSendTypes.ZorunluMailler, email.email, title, email.firstname + " " + email.lastname + " " + html, true);
            }
        }

        private void PersonAssesment()
        {
            try
            {
                #region performansdegerlendirme 
                var personeller = _persons.Where(a => a.JobStartDate != null && a.IsWorking == true).ToArray();
                var personels = personeller.Where(a => a.JobStartDate.Value.AddDays(53).Date == DateTime.Now.Date || a.JobStartDate.Value.AddDays(173).Date == DateTime.Now.Date ||
                a.JobStartDate.Value.AddDays(180).Date == DateTime.Now.Date || a.JobStartDate.Value.AddDays(60).Date == DateTime.Now.Date);
                var insankaynaklari = _db.GetSH_UserByRoleId(SHRoles.IKYonetici).FirstOrDefault();

                foreach (var person in personels)
                {
                    try
                    {
                        var personDept = _db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(person.id, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

                        if (person.JobStartDate.Value.AddDays(60).Date == DateTime.Now.Date || person.JobStartDate.Value.AddDays(180).Date == DateTime.Now.Date)
                        {
                            if (personDept.Manager1 != null)
                            {
                                var manager1 = _db.GetSH_UserById((Guid)personDept.Manager1);
                                var mesaj = string.Format(@"<p><strong>{0}</strong><br/> Adlı Personelin {1} aylık Performans Değerlendirme Formunun bugün doldurulması gerekiyor, sistem tarafından form kaydı oluşturulmuştur.</p>",
                                person.firstname + " " + person.lastname, (person.JobStartDate.Value.AddDays(60).Date == DateTime.Now.Date) ? short.Parse("2") : short.Parse("6"));

                                new Email().Template("Template1", "degerlendirme.jpg", _tenant.TenantName + " WORKOFTIME PERFORMANS DEĞERLENDİRME FORMU UYARISI", mesaj)
                                  .Send((Int16)EmailSendTypes.Degerlendirme, manager1.email, string.Format("{0} | {1}", _tenant.TenantName + " WORKOFTIME", "PERFORMANS DEĞERLENDİRME FORMU"), true);
                            }
                        }
                        else if (person.JobStartDate.Value.AddDays(53).Date == DateTime.Now.Date || person.JobStartDate.Value.AddDays(173).Date == DateTime.Now.Date)
                        {
                            var dbresult = _db.InsertINV_CompanyPersonAssessment(new INV_CompanyPersonAssessment
                            {
                                IdUser = person.id,
                                changed = DateTime.Now,
                                AssessmentType = (person.JobStartDate.Value.AddDays(53).Date == DateTime.Now.Date) ? (short)2 : (short)6,
                                AssessmentCode = BusinessExtensions.B_GetIdCode(),
                                ApproveStatus = (int)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici1Degerlendirme,
                                Manager1Approval = personDept.Manager1
                            });

                            if (insankaynaklari != null)
                            {
                                var mesaj = string.Format(@"<p><strong>{0}</strong><br/> Adlı Personelin {1} aylık Performans Değerlendirme Formunun doldurulmasına 1 hafta kaldı sistem tarafından form kaydı oluşturulmuştur.</p>",
                                    person.firstname + " " + person.lastname, (person.JobStartDate.Value.AddDays(53).Date == DateTime.Now.Date) ? short.Parse("2") : short.Parse("6"));

                                new Email().Template("Template1", "degerlendirme.jpg", _tenant.TenantName + " WORKOFTIME PERFORMANS DEĞERLENDİRME FORMU UYARISI", mesaj)
                                .Send((Int16)EmailSendTypes.Degerlendirme, insankaynaklari.email, string.Format("{0} | {1}", _tenant.TenantName + " WORKOFTIME", "PERFORMANS DEĞERLENDİRME FORMU"), true);
                            }

                            if (personDept.Manager1 != null)
                            {
                                var manager1 = _db.GetSH_UserById((Guid)personDept.Manager1);
                                var mesaj = string.Format(@"<p><strong>{0}</strong><br/> Adlı Personelin {1} aylık Performans Değerlendirme Formunun bugün doldurulması gerekiyor, sistem tarafından form kaydı oluşturulmuştur.</p>",
                                person.firstname + " " + person.lastname, (person.JobStartDate.Value.AddDays(60).Date == DateTime.Now.Date) ? short.Parse("2") : short.Parse("6"));

                                new Email().Template("Template1", "degerlendirme.jpg", _tenant.TenantName + " | WORKOFTIME PERFORMANS DEĞERLENDİRME FORMU UYARISI", mesaj)
                                  .Send((Int16)EmailSendTypes.Degerlendirme, manager1.email, string.Format("{0} | {1}", _tenant.TenantName, "PERFORMANS DEĞERLENDİRME FORMU"), true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Performans değerlendirme kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1}) ", _tenant.TenantName, _tenant.TenantCode, ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error("Performans değerlendirme kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1}) ", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }
        }

        private void ProjectTimeline()
        {
            try
            {
                #region zamancizelgesi 
                var activetimeline = _timeline.Where(a => a.EndDate.Value.Date == DateTime.Now.Date || a.EndDate.Value.AddDays(-7).Date == DateTime.Now.Date).ToArray();
                foreach (var item in activetimeline)
                {

                    var users = _db.GetPRJ_ProjectTimelinePersonTimelineId(item.id);
                    var _data = _db.GetVWPRJ_ProjectTimelineById(item.id);
                    var userList = new List<SH_User>();
                    foreach (var user in users)
                    {
                        var mail = _db.GetSH_UserById(user.IdUser.Value);
                        userList.Add(mail);

                    }
                    SendEmail(userList, "WorkOfTime Proje Zaman Çizelgesi Uyarısı.",
                                  "<span style='text-aling:center'>{0} adlı projenin son tarihi {1} olan {2}({3}) teslimine <b>{4}</b> saat kalmıştır.</span>",
                                _data.Project_Title, string.Format("{0}:{0:dd.MM.yyyy HH:mm}", _data.EndDate), _data.Name, _data.Description, Math.Round((_data.EndDate - DateTime.Now).Value.TotalHours, 2));

                }
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error("Timeline kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1}) ", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }

        }

        private void PersonBirthday()
        {
            try
            {
                #region dogumgunu
                var activebirthday = _persons.Where(a => a.birthday.HasValue && a.birthday.Value.Day == DateTime.Now.Day && a.birthday.Value.Month == DateTime.Now.Month && a.IsWorking == true).ToArray();
                foreach (var item in activebirthday)
                {

                    var ikmail = _db.GetSH_UserByRoleId(SHRoles.IKYonetici).Select(a => a.email);
                    var mesaj = string.Format(@"Sn. <strong>{0}</strong><br/> Doğum gününüzü kutlar birlikte sağlıklı, başarı dolu seneler dileriz.", item.firstname + " " + item.lastname);
                    new Email().Template("Template1", "birthday.jpg", "Doğum Günü Tebrik", mesaj)
                       .Send((Int16)EmailSendTypes.ZorunluMailler, item.email, string.Format("{0} | {1}", _tenant.TenantName, "Doğum Günü Tebrik"), true, null, null, ikmail.ToArray());

                }
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error("Doğum günü kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1}) ", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }
        }

        private void PersonWorkingStart()
        {
            try
            {
                _persons = _db.GetVWSH_UserMyPerson();
                var workDateGreeting = _persons.Where(a => a.IsWorking == true && a.JobStartDate != null && (a.JobStartDate.Value.Day == DateTime.Now.Day && a.JobStartDate.Value.Month == DateTime.Now.Month));
                foreach (var item in workDateGreeting)
                {
                    try
                    {
                        var WorkTime = DateTime.Now.Year - item.JobStartDate.Value.Year;
                        if (WorkTime > 0)
                        {
                            var mesaj = string.Format(@"<strong>{0}</strong> bünyesinde <strong>{1}</strong> yılı geride bıraktınız. Bu süre içerisindeki gayretli ve özverili çalışmalarınızdan dolayı teşekkür eder, başarılarınızın devamını dileriz...", item.Company_Title, WorkTime);
                            _email.Template("Template1", "103.jpg", "Merhaba " + item.FullName, mesaj)
                               .Send((Int16)EmailSendTypes.ZorunluMailler, item.email, string.Format("{0} | {1}", _tenant.TenantName, "İşe Giriş Yıldönümü Tebriği"), true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("iş'e giriş tebriği kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1}) ", _tenant.TenantName, _tenant.TenantCode, ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Log.Error("iş'e giriş tebriği kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }
        }


        private void TenderControl()
        {
            try
            {
                var tenders = _db.GetVWCMP_Tender().Where(x => x.status == (int)EnumCMP_TenderStatus.CevapBekleniyor && x.created < DateTime.Now.AddHours(23).AddMinutes(59));
                var roles = _db.GetSH_UserRoleByRoleId(new Guid(SHRoles.SatinAlmaOnaylayici));
                var users = new List<VWSH_User>();
                if (roles.Any())
                {
                    users = _db.GetVWSH_UserByIds(roles.Where(x => x.userid.HasValue).Select(x => x.userid.Value).ToArray()).ToList();
                }

                foreach (var tender in tenders)
                {
                    foreach (var user in users)
                    {
                        var text = "<h3>Sayın " + user.FullName + "</h3>";
                        text += "<p>" + tender.rowNumber + " kodlu onayınızı bekleyen teklifin üzerinden 24 saat geçmiştir. </p>";
                        text += "<p>Teklif detayını görüntülemek ve işlem yapmak için <a href='" + _url + "/CMP/VWCMP_Tender/DetailSelling?id=" + tender.id + "'>tıklayınız.</a> </p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "satinalma.jpg", TenantConfig.Tenant.TenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, user.email, "Teklif Oluşturuldu", true);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void PersonCalendar()
        {
            try
            {
                var activeEvents = _events.Where(a => string.Format("{0}:{0:dd.MM.yyyy HH:mm}", a.StartDate.Value.AddSeconds(-15)) == DateTime.Now.ToString("dd.MM.yyyy HH:mm")).ToArray();
                foreach (var item in activeEvents)
                {
                    try
                    {
                        var myevent = _db.GetVWINV_CompanyPersonCalendarById(item.id);
                        var persons = _db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(item.id);
                        var userId = persons.Select(x => x.IdUser != null ? x.IdUser.Value : Guid.NewGuid()).ToArray();
                        var emails = _db.GetSH_UserByIds(userId).ToArray();
                        SendEmail(emails, "WorkOfTime Gündem Bilgilendirmesi.",
                            "<span style='text-aling:center'>{0} - {1} Tarihleri arasında bir gündeminiz bulunmaktadır. <strong>Tipi : </strong> {2} <br/> <strong>Başlık : </strong> {3} <br/>  <strong>Açıklama : </strong> {4} <br/>  </span>", string.Format("{0}:{0:dd.MM.yyyy HH:mm}", myevent.StartDate), string.Format("{0}:{0:dd.MM.yyyy HH:mm}", myevent.EndDate), myevent.Type_Title, myevent.Title, myevent.Description);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Personel takvim kontrolleri yapılırken sorunlar oluştu.Mesaj: {2} ({0} {1}) ", _tenant.TenantName, _tenant.TenantCode, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Personel takvim kontrolleri yapılırken sorunlar oluştu. Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }
        }

        private void PerDayMailService()
        {
            try
            {
                if (_tenant.TenantCode.HasValue)
                {
                    var cog = new Config(_tenant.TenantCode.Value);
                    var mail = new PerDayMailService(_db, _url).GetHTML();
                    if (cog.MailingUsers == null)
                    {
                        Log.Error("{0} firmasında mail gönderilecek adres bulunamadı", _tenant.TenantName);
                    }

                    var sendUsers = string.Join(";", cog.MailingUsers);
                    if (!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(sendUsers))
                    {
                        new Email().Template("Template1", "bos.png", "Bugünkü işleriniz ile ilgili birkaç bilgi vermek istedik;", mail)
                             .Send((Int16)EmailSendTypes.ZorunluMailler, sendUsers, string.Format("{0} | {1}", _tenant.TenantName + " WorkOfTime ", "Gün Başı Maili"), true, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Gün başı maili gönderilirken sorun oluştu.. Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }

        }

        private void EndOfDayMailService()
        {
            try
            {
                if (_tenant.TenantCode.HasValue)
                {
                    var cog = new Config(_tenant.TenantCode.Value);
                    var mail = new EndOfDayMailService(_db, _url).GetDayEndHTML();
                    if (cog.MailingUsers == null)
                    {
                        Log.Error("{0} firmasında mail gönderilecek adres bulunamadı", _tenant.TenantName);
                    }
                    var sendUsers = string.Join(";", cog.MailingUsers);
                    if (!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(sendUsers))
                    {
                        new Email().Template("Template1", "bos.png", "Bugünkü işleriniz ile ilgili birkaç bilgi vermek istedik;", mail)
                             .Send((Int16)EmailSendTypes.ZorunluMailler, sendUsers, string.Format("{0} | {1}", _tenant.TenantName + " WorkOfTime ", "Gün Sonu Maili"), true, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Gün sonu maili gönderilirken sorun oluştu.. Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }

        }

        private void WeeklyMailService()
        {
            try
            {
                if (_tenant.TenantCode.HasValue)
                {
                    var cog = new Config(_tenant.TenantCode.Value);
                    var mail = new WeeklyMailService(_db, _url).GetDayEndHTML();
                    if (cog.MailingUsers == null)
                    {
                        Log.Error("{0} firmasında mail gönderilecek adres bulunamadı", _tenant.TenantName);
                    }
                    var sendUsers = string.Join(";", cog.MailingUsers);
                    if (!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(sendUsers))
                    {
                        new Email().Template("Template1", "bos.png", "Hafta boyunca gerçekleşen işleriniz ile ilgili birkaç bilgi vermek istedik;", mail)
                             .Send((Int16)EmailSendTypes.ZorunluMailler, sendUsers, string.Format("{0} | {1}", _tenant.TenantName + " WorkOfTime ", "Hafta Sonu Maili"), true, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Haftalık mail gönderilirken sorun oluştu.. Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }

        }

        public void Dispose()
        {
            start = false;
            Log.Info(_tenant.TenantName + " " + _tenant.TenantCode + " Kiracısı için ajan durduruldu.");
        }
    }
}
