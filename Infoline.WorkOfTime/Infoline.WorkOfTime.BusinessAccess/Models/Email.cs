using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public enum EmailSendTypes
    {
        //0-20 arası tüm izin ile ilgili mail durumları
        #region İzinler 
        [Generic("group", "İzin", "name", "İzin Onay Mailleri", "title", "İzinler ile ilgili onay maillerinin tarafıma gönderilmesi")]
        IzinOnaylari = 0,
        [Generic("group", "İzin", "name", "İzin Süreç Tamamlama Mailleri", "title", "İzin süreci tamamlama maillerinin tarafıma gönderilmesi")]
        IzinSurecTamamlama = 1,

        #endregion
        //20-40 arası tüm görevlendirme ile ilgili mail durumları
        #region Görevlendirmeler 
        [Generic("group", "Görevlendirme", "name", "Görevlendirme Onay Mailleri", "title", "Görevlendirme ile ilgili onay maillerinin tarafıma gönderilmesi")]
        GorevlendirmeOnaylari = 20,
        [Generic("group", "Görevlendirme", "name", "Görevlendirme Süreç Tamamlama Mailleri", "title", "Görevlendirme süreci tamamlama maillerinin tarafıma gönderilmesi")]
        GorevlendirmeSurecTamamlama = 21,

        #endregion
        //40-60 arası tüm masraf ile ilgili mail durumları
        #region Masraflar
        [Generic("group", "Masraf", "name", "Masraf Onay Mailleri", "title", "Masraflar ile ilgili tüm onay/bilgilendirme maillerinin tarafıma gönderilmesi")]
        MasrafOnay = 40,
        #endregion
        //60-80 arası tüm avans ile ilgili mail durumları
        #region Avanslar 
        [Generic("group", "Avans", "name", "Avans Onay Mailleri", "title", "Avanslar ile ilgili tüm onay/bilgilendirme maillerinin tarafıma gönderilmesi")]
        AvansOnay = 60,

        #endregion
        //80-100 arası tüm değerlendirme ile ilgili mail durumları
        #region Değerlendirmeler 
        [Generic("group", "Değerlendirme", "name", "Personel Değerlendirme Mailleri", "title", "Tüm personel değerlendirme ile ilgili maillerinin tarafıma gönderilmesi")]
        Degerlendirme = 80,

        #endregion
        //100-120 arası tüm personel aday işlemleri ile ilgili mail durumları
        #region Personel Aday işlemleri 
        [Generic("group", "Personel Aday İşlemleri", "name", "Aday Personel Mülakat Mailleri", "title", "Tüm personel adayı işlemleri ile ilgili maillerinin tarafıma gönderilmesi")]
        PersonelAdayMulakat = 100,

        #endregion
        //120-140 arası tüm potansiyel ile ilgili mail durumları
        #region Potansiyel
        [Generic("group", "Potansiyel", "name", "Potansiyel Toplantı Mailleri", "title", "Potansiyel randevu/toplantı maillerinin tarafıma gönderilmesi")]
        Toplanti = 120,
        [Generic("group", "Potansiyel", "name", "Potansiyel Yeni Müşteri Mailleri", "title", "Potansiyel yeni müşteri tanımlama maillerinin tarafıma gönderilmesi")]
        YeniMusteri = 121,
        [Generic("group", "Potansiyel", "name", "Potansiyel Teklif Mailleri", "title", "Potansiyel teklif maillerinin tarafıma gönderilmesi")]
        Teklif = 122,

        #endregion
        //140-160 arası tüm satış sipariş/Satın alma ile ilgili mail durumları
        #region Satış Sipariş / Satın ALma
        [Generic("group", "Satış/Sipariş/Satın Alma", "name", "Sipariş Mailleri", "title", "")]
        YeniSiparis = 140,
        [Generic("group", "Satış/Sipariş/Satın Alma", "name", "Teklif Mailleri", "title", "")]
        SiparisTeklif = 141,
        [Generic("group", "Satış/Sipariş/Satın Alma", "name", "Satın Alma Mailleri", "title", "")]
        SatinAlma = 142,
        [Generic("group", "Satış/Sipariş/Satın Alma", "name", "Satın Alma Teklif Mailleri", "title", "")]
        SatinAlmaTeklif = 143,
        #endregion
        //160-180 arası tüm stok ile ilgili mail durumları
        #region Stok
        [Generic("group", "Stok", "name", "Stok Hareket Mailleri", "title", "")]
        Zimmet = 160,

        #endregion
        //180-200 arası tüm saha görev ile ilgili mail durumları
        #region Saha Görev
        [Generic("group", "Saha Görevi", "name", "Saha Operasyon Mailleri", "title", "")]
        Operasyon = 180,
        [Generic("group", "Saha Görevi", "name", "Saha Çözüm Mailleri", "title", "")]
        Cozum = 181,

        #endregion
        //200-220 arası tüm yardım masası ile ilgili mail durumları
        #region Yardım Masası
        [Generic("group", "Yardım Masası", "name", "Yardım Talep Mailleri", "title", "")]
        YardimTalep = 200,
        [Generic("group", "Yardım Masası", "name", "Yardım Talep Çözüm Mailleri", "title", "")]
        YardimCozum = 201,

        #endregion
        //220-240 arası tüm dosya paylaşım ile ilgili mail durumları
        #region Dosya Paylaşım
        [Generic("group", "Dosya", "name", "Dosya Paylaşım Mailleri", "title", "")]
        Dosya = 220,
        #endregion
        //240-260 arası fatura ile ilgili mailler
        #region Fatura
        [Generic("group", "Fatura", "name", "Fatura/İrsaliye Mailleri", "title", "")]
        Fatura = 240,
        #endregion
        //260-280 arası duyuru/etkinlik mailleri
        #region Duyuru
        [Generic("group", "Duyuru/Etkinlik", "name", "Duyuru/Etkinlik Mailleri", "title", "Tüm duyuru/etkinlik/not/bilgilendirme vs. maillerin tarafıma gönderilmesi")]
        DuyuruEtkinlik = 260,
        #endregion
        //270-280 arası doküman mailleri 
        #region Doküman
        [Generic("group", "Doküman Yonetimi", "name", "Revizyon Talebi Mailleri", "title", "Tüm revizyon talep maillerin tarafıma gönderilmesi")]
        RevizyonTalebi = 270,
        #endregion
        //300 burası seçime bağlı olmayacak.
        #region Zorunlu
        [Generic("group", "Zorunlu", "name", "Zorunlu", "title", "Şifre/Bilgilendirme değişiklik gibi bildirimlerin gönderilmesi")]
        ZorunluMailler = 300,
        #endregion
    }

    public class Sender
    {
        private SmtpClient _smtpClient = new SmtpClient();
        public MailMessage _mailMessage = new MailMessage();
        private string _username { get; set; }
        private string _tenantname { get; set; }
        private string _password { get; set; }
        private bool _isBodyHtml { get; set; }
        private string _content { get; set; }
        private WorkOfTimeDatabase _db { get; set; }
        private AlternateView _alternativeView { get; set; }
        private static object obj = new object();

        public Sender(int? tenantCode = null)
        {
            _defaults(tenantCode);
        }

        public Sender(string mesaj, bool isbodyHtml, int? tenantCode = null)
        {
            _defaults(tenantCode);
            _content = mesaj;
            _isBodyHtml = isbodyHtml;
        }

        public Sender(AlternateView view, int? tenantCode = null)
        {
            _defaults(tenantCode);
            _alternativeView = view;
            _isBodyHtml = true;
        }

        public void Send(int enumKey, string email, string baslik, bool asenkron = true, string[] cc = null, string[] bcc = null, string[] files = null, bool hasWarning = false)
        {
            var ts = Task.Run(() => _send(enumKey, email, baslik, cc, files, bcc, hasWarning));
            if (!asenkron)
            {
                ts.Wait();
            }

        }

        public void SendCalendar(string email, string baslik, string mesaj, DateTime startDate, DateTime endDate, string file = "", bool isBodyHtml = true)
        {
            _mailMessage.To.Clear();
            _mailMessage.Body = mesaj;
            if (email.Split(';').Count() > 0)
            {
                foreach (var mailAdr in email.Split(';'))
                {
                    _mailMessage.To.Add(mailAdr);
                }
            }
            else
            {
                _mailMessage.To.Add(email);
            }

            _mailMessage.IsBodyHtml = isBodyHtml;
            _mailMessage.From = new System.Net.Mail.MailAddress(_username, _tenantname, Encoding.UTF8);
            _mailMessage.Subject = baslik;

            if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
            {
                System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                _mailMessage.Attachments.Add(item);
            }

            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");

            //PRODID: identifier for the product that created the Calendar object
            str.AppendLine("PRODID:-//ABC Company//Outlook MIMEDIR//EN");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("METHOD:REQUEST");

            str.AppendLine("BEGIN:VEVENT");

            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", TimeZoneInfo.ConvertTimeToUtc(startDate).ToString("yyyyMMddTHHmmssZ")));//TimeZoneInfo.ConvertTimeToUtc("BeginTime").ToString("yyyyMMddTHHmmssZ")));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", TimeZoneInfo.ConvertTimeToUtc(DateTime.UtcNow).ToString("yyyyMMddTHHmmssZ")));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", TimeZoneInfo.ConvertTimeToUtc(endDate).ToString("yyyyMMddTHHmmssZ")));//TimeZoneInfo.ConvertTimeToUtc("EndTime").ToString("yyyyMMddTHHmmssZ")));

            //str.AppendLine(string.Format("LOCATION: {0}", "Location"));

            // UID should be unique.
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", _mailMessage.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", _mailMessage.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", _mailMessage.Subject));

            str.AppendLine("STATUS:CONFIRMED");
            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:Accept");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");

            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", _mailMessage.From.Address));
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", _mailMessage.To[0].DisplayName, _mailMessage.To[0].Address));

            str.AppendLine("END:VCALENDAR");
            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
            if (ct.Parameters != null)
            {
                ct.Parameters.Add("method", "REQUEST");
                ct.Parameters.Add("name", "meeting.ics");
            }
            var avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
            _mailMessage.AlternateViews.Add(avCal);

            try
            {
                _smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                // hatamesaji = ex.Message;
            }
        }

        private void _send(int enumKey, string email, string title, string[] cc = null, string[] files = null, string[] bcc = null, bool hasWarning = false)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            _mailMessage.Body = _content;
            _mailMessage.IsBodyHtml = _isBodyHtml;
            _mailMessage.From = new System.Net.Mail.MailAddress(_username, _tenantname, Encoding.UTF8);
            _mailMessage.Subject = title;


            if (_mailMessage.IsBodyHtml && _alternativeView != null)
            {
                _mailMessage.AlternateViews.Add(_alternativeView);
            }

            foreach (var item in (email ?? "").Split(';').Where(a => _isValidEmail(a.Trim())).Select(a => a.Trim()))
            {
                var res = _db.GetVWSYS_BlockMailByMailAndType(item, enumKey);
                if (res == null)
                {
                    _mailMessage.To.Add(item);
                }
                else
                {
                    _db.InsertSYS_Email(new SYS_Email
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        SendingIsBodyHtml = _isBodyHtml,
                        SendingMail = email,
                        SendingTitle = title,
                        SendingMessage = _content,
                        Result = "Mail için kullanıcı tarafından izin verilmemiş",
                        Status = false
                    });
                }
            }

            if (_mailMessage.To.Count() == 0)
            {
                return;
            }

            foreach (var item in cc ?? new string[] { })
            {
                _mailMessage.CC.Add(item);
            }

            //var datasss = _iysControl(email);


            foreach (var item in bcc ?? new string[] { })
            {
                _mailMessage.Bcc.Add(item);
            }

            var streams = new List<FileStream>();
            foreach (var file in files ?? new string[] { })
            {
                try
                {
                    if (file.StartsWith("http"))
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            var stream = webClient.OpenRead(file);
                            FileStream fs = stream as FileStream;
                            string fileName = null;
                            if (fs != null)
                            {
                                fileName = fs.Name;
                            }

                            if (string.IsNullOrEmpty(fileName))
                            {
                                try
                                {
                                    var distr = webClient.ResponseHeaders["content-disposition"];
                                    fileName = new ContentDisposition(distr)?.FileName;
                                }
                                catch { }
                            }

                            if (string.IsNullOrEmpty(fileName))
                            {
                                try
                                {
                                    string contenttype = webClient.ResponseHeaders["content-type"];
                                    fileName = DateTime.Now.ToString("yyyyMMdd-HHMMss") + FileHelper.GetExtension(contenttype.Split(';').FirstOrDefault());
                                }
                                catch { }
                            }

                            _mailMessage.Attachments.Add(new Attachment(stream, fileName));
                        }
                    }
                    else
                    {
                        var mimeType = MimeMapping.GetMimeMapping(file);
                        var ct = new ContentType(mimeType);
                        var data = new Attachment(file, ct);
                        data.Name = Path.GetFileName(file);
                        _mailMessage.Attachments.Add(data);
                    }
                }
                catch { }
            }

            var rs = new ResultStatus();
            try
            {
                lock (obj)
                {
                    _smtpClient.Send(_mailMessage);
                    foreach (var file in streams)
                    {
                        file.Dispose();
                    }
                }
                rs.result = true;
                rs.message = "Gönderim işlemi başarılı.";
            }
            catch (Exception ex)
            {
                rs.result = false;
                rs.message = ex.Message;
                Log.Error(ex.Message);
            }

            if (_alternativeView != null)
            {
                using (StreamReader reader = new StreamReader(_alternativeView.ContentStream))
                {
                    _alternativeView.ContentStream.Position = 0;
                    _content = reader.ReadToEnd();
                }
            }
            if (!hasWarning)
            {
                var rss = _db.InsertSYS_Email(new SYS_Email
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    SendingIsBodyHtml = _isBodyHtml,
                    SendingMail = email,
                    SendingTitle = title,
                    SendingMessage = _content,
                    Result = rs.message,
                    Status = rs.result
                });
            }

        }

        private bool _isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void _defaults(int? tenantCode = null)
        {
            var tenant = tenantCode.HasValue ? TenantConfig.GetTenantByTenantCode(tenantCode) : TenantConfig.Tenant;
            _db = tenant.GetDatabase();
            _tenantname = tenant.TenantName;
   
            if(TenantConfig.Tenant.hasUser == true)
            {
                _username = tenant.MailUser.Split('@').FirstOrDefault();
            }
            else
            {
                _username = tenant.MailUser;
            }
            
            _password = tenant.MailPassword;
            _smtpClient.Host = tenant.MailHost;
            _smtpClient.Port = tenant.MailPort ?? 587;
            _smtpClient.EnableSsl = tenant.MailSSL ?? true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.Credentials = new System.Net.NetworkCredential(_username, _password);


        }

        private bool _iysControl(string email)
        {
            var tenant = TenantConfig.Tenant.Config;
            if (tenant.IysInformations == null)
            {
                return true;
            }
            if (string.IsNullOrEmpty(tenant.IysInformations.userName))
            {
                return true;
            }

            try
            {
                //Servise bağlanılıyor.
                var services = new IYServices(tenant.IysInformations.userName, tenant.IysInformations.password, tenant.IysInformations.code, tenant.IysInformations.brandCode, tenant.IysInformations.retailerCode);

                //Token alınıyor.
                services.TakeToken();
                services.RefreshToken();

                //İys sisteminde kontrol için obje oluşturuluyor.
                var iys = new IYS_IndividualLeaveQueryRequest
                {
                    recipient = email,
                    type = "EPOSTA",
                    recipientType = "BIREYSEL"
                };

                //İys de kayıt var mı kontrolü yapılıyor.
                var iysServices = services.IndividualLeaveQuery(iys);
                if (iysServices.status == "ONAY")
                {
                    return true;
                }

                if (iysServices.status == "RET")
                {
                    return false;
                }

                //İys de kayıt yok ise yeni kayıtiçin obje oluşturuluyor.
                var newRegister = new IYS_SingularPermitRequest
                {
                    consentDate = (DateTime.Now.Year) + "-" +
                                    (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()) + "-" +
                                    (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString()) + " " +
                                    (DateTime.Now.Hour < 10 ? "0" + DateTime.Now.Hour : DateTime.Now.Hour.ToString()) + ":" +
                                    (DateTime.Now.Minute < 10 ? "0" + DateTime.Now.Minute : DateTime.Now.Minute.ToString()) + ":" +
                                    (DateTime.Now.Second < 10 ? "0" + DateTime.Now.Second : DateTime.Now.Second.ToString()),
                    source = "HS_WEB",
                    recipient = email,
                    recipientType = "BIREYSEL",
                    status = "ONAY",
                    type = "EPOSTA"
                };

                //İys sistemine kayıt atılıyor.
                var res = services.InsertPermit(newRegister);

                if (!string.IsNullOrEmpty(res.transactionId))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public class Email
    {
        private int? _tenantCode { get; }

        public Email(int? tenantCode = null)
        {
            _tenantCode = tenantCode;
        }

        public Sender Template(string templateName, string image, string title, string content)
        {
            var appDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var templateFileDirectory = string.Format(@"{0}\App_Data\MailTemplate\{1}\index.html", appDirectory, templateName);
            templateFileDirectory = templateFileDirectory.Remove(0, 6);
            string html = System.IO.File.ReadAllText(templateFileDirectory);

            var ten = _tenantCode == null ? TenantConfig.Tenant : TenantConfig.GetTenantByTenantCode(_tenantCode);
            var siteUrl = ten.GetWebUrl();
            var resourceCollection = new List<LinkedResource>();

            html = html.Replace("{{title}}", title);
            html = html.Replace("{{content}}", content);
            html = html.Replace("{{siteUrl}}", siteUrl);

            if (!string.IsNullOrEmpty(image) && html.IndexOf("{{imagePath}}") > 0)
            {
                var baseImagePath = string.Format(@"{0}\App_Data\ContentImages\{1}", appDirectory, image);
                baseImagePath = baseImagePath.Remove(0, 6);
                html = html.Replace("{{imagePath}}", "cid:0");
                resourceCollection.Add(new LinkedResource(FileToImageStream(baseImagePath), new ContentType(MimeMapping.GetMimeMapping(baseImagePath)))
                {
                    ContentId = "0"
                });
            }

            var imgCount = 1;
            foreach (Match m in Regex.Matches(html, "<img(?<value>.*?)>"))
            {
                var imgContent = m.Groups["value"].Value.ToLower();
                try
                {
                    var imagePath = string.Join("", Regex.Match(imgContent, "((src)=['\"])(.*?)['\"]").Value.Replace("src=", "").Split(new char[] { '\'', '"' }));
                    var imageDirectoryPath = HttpUtility.UrlDecode(string.Join(@"\", imagePath.Split('/')));
                    var serveImagePath = string.Format(@"{0}\App_Data\MailTemplate\{1}\{2}", appDirectory, templateName, imageDirectoryPath);
                    serveImagePath = serveImagePath.Remove(0, 6);
                    if (!File.Exists(serveImagePath))
                    {
                        continue;
                    }

                    html = html.Replace(imagePath, "cid:" + imgCount);
                    var tempResource = new LinkedResource(FileToImageStream(serveImagePath), new ContentType(MimeMapping.GetMimeMapping(serveImagePath)))
                    {
                        ContentId = imgCount.ToString()
                    };
                    resourceCollection.Add(tempResource);

                    imgCount++;

                }
                catch (Exception)
                {
                    continue;
                }

            }

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            foreach (var item in resourceCollection)
            {
                alternateView.LinkedResources.Add(item);
            }

            return new Sender(alternateView, _tenantCode);
        }

        public void Send(int enumKey, string email, string baslik, string mesaj, bool isBodyHtml = false, bool asenkron = false, string[] cc = null, string[] bcc = null, string[] files = null, bool hasWarning = false)
        {
            new Sender(mesaj, isBodyHtml, _tenantCode).Send(enumKey, email, baslik, asenkron, cc, bcc, files, hasWarning);
        }

        public void SendCalendar(string email, string baslik, string mesaj, DateTime startDate, DateTime endDate, string file = "", bool isBodyHtml = true)
        {
            new Sender(_tenantCode).SendCalendar(email, baslik, mesaj, startDate, endDate, file, isBodyHtml);
        }

        private static MemoryStream FileToImageStream(string filePath)
        {
            using (Image image = Image.FromFile(filePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    return ms;
                }
            }
        }
    }
}
