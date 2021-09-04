using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.Helper
{

    public class E_MAIL : IDisposable
    {

        public string hatamesaji = "";
        MailMessage mail = new System.Net.Mail.MailMessage();
        SmtpClient cl = new System.Net.Mail.SmtpClient();
        private string _userName;
        public E_MAIL()
        {

            string host = "", password = "";
            bool ssl = true;
            int port = 0;

            if (System.Configuration.ConfigurationManager.AppSettings["mailHost"] != null)
            {
                host = System.Configuration.ConfigurationManager.AppSettings["mailHost"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailPort"] != null)
            {
                port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mailPort"]);
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailUser"] != null)
            {
                _userName = System.Configuration.ConfigurationManager.AppSettings["mailUser"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailPass"] != null)
            {
                password = System.Configuration.ConfigurationManager.AppSettings["mailPass"].ToString();
            }

            //if (System.Configuration.ConfigurationManager.AppSettings["mailSsl"] != null)
            //{
            //    ssl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["mailSsl"]);
            //}

            cl.Host = host;
            cl.Port = port;
            cl.EnableSsl = ssl;
            cl.DeliveryMethod = SmtpDeliveryMethod.Network;
            cl.Credentials = new System.Net.NetworkCredential(_userName, password);
            mail.IsBodyHtml = true;
            mail.Priority = System.Net.Mail.MailPriority.High;



        }

        public E_MAIL(string Host, int Port, string userName, string password, bool ssl, bool isBodyHtml = true, string domain = null)
        {
            _userName = userName;
            cl.Host = Host;
            cl.Port = Port;
            cl.EnableSsl = ssl;
            cl.DeliveryMethod = SmtpDeliveryMethod.Network;
            cl.Credentials = new System.Net.NetworkCredential(userName, password, domain);
            mail.IsBodyHtml = isBodyHtml;
            mail.Priority = System.Net.Mail.MailPriority.High;
        }

        public Task<bool> TaskSend(string email, string baslik, string mesaj, string file = "", bool isBodyHtml = true)
        {
            return Task.Run(() =>
            {
                mail.Body = mesaj;
                mail.To.Add(email);
                mail.IsBodyHtml = isBodyHtml;
                mail.From = new System.Net.Mail.MailAddress(_userName, _userName);
                mail.Subject = baslik;

                if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                {
                    System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                    mail.Attachments.Add(item);
                }
                try
                {
                    cl.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    hatamesaji = ex.Message;
                }
                return false;

            });
        }

        public void SendByThread(string email, string baslik, string mesaj, string file = "", bool isBodyHtml = true)
        {
            new Thread(new ThreadStart(delegate
            {
                mail.Body = mesaj;

                if (email.Split(';').Count() > 0)
                {
                    foreach (var mailAdr in email.Split(';'))
                    {
                        mail.To.Add(mailAdr);
                    }
                }
                else
                {
                    mail.To.Add(email);
                }

                mail.IsBodyHtml = isBodyHtml;
                mail.From = new System.Net.Mail.MailAddress(_userName, _userName);
                mail.Subject = baslik;

                if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                {
                    System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                    mail.Attachments.Add(item);
                }
                try
                {
                    LogWrite("Mail Gönderiliyor...");
                    cl.Send(mail);
                }
                catch (Exception ex)
                {
                    LogWrite("HATA : /t" + ex.Message.ToString());
                    hatamesaji = ex.Message;
                }
            }
            )).Start();
        }
        public void Send(string email, string baslik, string mesaj, string file = "", bool isBodyHtml = true)
        {

            mail.To.Clear();
            mail.Body = mesaj;
            if (email.Split(';').Count() > 0)
            {
                foreach (var mailAdr in email.Split(';'))
                {
                    mail.To.Add(mailAdr);
                }
            }
            else
            {
                mail.To.Add(email);
            }

            mail.IsBodyHtml = isBodyHtml;
            mail.From = new System.Net.Mail.MailAddress(_userName, _userName);
            mail.Subject = baslik;

            if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
            {
                System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                mail.Attachments.Add(item);
            }
            try
            {
                LogWrite("Mail Gönderiliyor...");
                cl.EnableSsl = true;
                cl.Send(mail);
            }
            catch (Exception ex)
            {
                LogWrite("HATA : /t" + ex.Message.ToString());
                hatamesaji = ex.Message;
            }
        }
        public void SendCalender(string email, string baslik, string mesaj, DateTime startDate, DateTime endDate, string file = "", bool isBodyHtml = true)
        {
            mail.To.Clear();
            mail.Body = mesaj;
            if (email.Split(';').Count() > 0)
            {
                foreach (var mailAdr in email.Split(';'))
                {
                    mail.To.Add(mailAdr);
                }
            }
            else
            {
                mail.To.Add(email);
            }

            mail.IsBodyHtml = isBodyHtml;
            mail.From = new System.Net.Mail.MailAddress(_userName, _userName);
            mail.Subject = baslik;

            if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
            {
                System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                mail.Attachments.Add(item);
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
            str.AppendLine(string.Format("DESCRIPTION:{0}", mail.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", mail.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", mail.Subject));

            str.AppendLine("STATUS:CONFIRMED");
            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:Accept");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");

            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", mail.From.Address));
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", mail.To[0].DisplayName, mail.To[0].Address));

            str.AppendLine("END:VCALENDAR");
            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
            if (ct.Parameters != null)
            {
                ct.Parameters.Add("method", "REQUEST");
                ct.Parameters.Add("name", "meeting.ics");
            }
            var avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
            mail.AlternateViews.Add(avCal);

            try
            {
                cl.Send(mail);
            }
            catch (Exception ex)
            {
                hatamesaji = ex.Message;
            }
        }
        public void Dispose()
        {
            mail.Dispose();
        }

        public void LogWrite(string str)
        {


            if (string.IsNullOrEmpty(str)) { return; }

            var path = "";
            if (HttpContext.Current != null)
                path = HttpContext.Current.Server.MapPath("\\log");
            else
                path = AppDomain.CurrentDomain.BaseDirectory + "log";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            if (string.IsNullOrEmpty(str)) return;
            var FILE = path + string.Format(@"\{0}-log.txt", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));


            using (StreamWriter sw = File.AppendText(FILE))
            {
                sw.WriteLine(DateTime.Now + ":" + str);
            }

        }

    }
}
