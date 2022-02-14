using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.OmixEntegrationApp.Utils
{
    public sealed class NotificationLogger : IDisposable
    {

        private static string ApplicationName
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["ApplicationName"] != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
                }

                return "ApplicationName Belirtilmemiş.";

            }
        }

        public static string LogMailUsers
        {
            get
            {

                if (System.Configuration.ConfigurationManager.AppSettings["NotificationLog"] != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["NotificationLog"].ToString();
                }

                return "seyit.tekce@infoline-tr.com";

            }
        }

        private static int? LastMailSendHour { get; set; }

        private static List<NotificationLog> Infos { get; set; }

        private static Thread TaskManager { get; set; }

        private static void SendMail()
        {

            try
            {
                var header = "<h1 style=\"text-align: center;\">UYGULAMA HATALARI</h1>";

                try
                {
                    header = "<h1 style=\"text-align: center;\">" +
                        (!String.IsNullOrEmpty(Environment.MachineName) ? Environment.MachineName : HttpContext.Current != null ? HttpContext.Current.Server.MachineName : "") +
                        "</h1>" +
                        header;
                }
                catch (Exception ex) { }
               
                header += "<h1 style=\"text-align: center; color: #f00;\">" + ApplicationName + "</h1>";

                var tbl = "<table border=\"1\" style=\"width: 100%;\">" +
                    "<thead>" +
                    "<tr>" +
                    "<th width=\"200\">" + "Tarih" + "</th>" +
                    "<th width=\"200\">" + "Distribütör" + "</th>" +
                    "<th>" + "Fatura Numarası" + "</th>" +
                    "<th>" + "Müşteri Kodu" + "</th>" +
                    "<th>" + "Müşteri Adı" + "</th>" +
                    "<th>" + "Imei" + "</th>" +
                    "<th>" + "Mesaj" + "</th>" +
                    "</tr>" +
                    "</thead>";
                tbl += "<tbody>" + String.Join("", Infos.Select(a => a.TableRow)) + "</tbody>" +
                    "</table>";

                new Email().Send(300, LogMailUsers, "OMİX ENTEGRASYON UYGULAMASI", header + tbl, true, false);

                Infos.Clear();
            }
            catch (Exception ex) { Console.WriteLine(ex); }

        }

        public static string GetKeyValues()
        {

            try
            {
                if (HttpContext.Current == null)
                {
                    return "";
                }

                return Infoline.Helper.Json.Serialize(HttpContext.Current.Request.Form.AllKeys
                    .ToDictionary(
                        key => key,
                        val => HttpContext.Current.Request.Form[val]
                    ));

            }
            catch
            {
                return "-";
            }

        }

        public static void SaveError(DateTime date, string message,PRD_EntegrationAction device)
        {

            try
            {

                if (Infos == null)
                {
                    Infos = new List<NotificationLog>();
                }



                var userStatus = HttpContext.Current != null && HttpContext.Current.Session != null
                    ? (PageSecurity)HttpContext.Current.Session["userStatus"]
                    : null;

                Infos.Add(new NotificationLog
                {
                    Message = message,
                    ConsolidationName=device.ConsolidationName,
                    CustomerOperatorCode=device.CustomerOperatorCode,
                    CustomerOperatorName=device.CustomerOperatorName,
                    DistributorName=device.DistributorName,
                    Imeı=device.Imei,
                    InvoiceNumber=device.InvoiceNumber,
                    Date = date,
                   
                });
                if (TaskManager == null)
                {

                    TaskManager = new Thread(new ThreadStart(delegate
                    {

                        if (!LastMailSendHour.HasValue)
                        {
                            LastMailSendHour = DateTime.Now.Hour - 1;
                        }

                        while (true)
                        {

                            if (LastMailSendHour < DateTime.Now.Hour && Infos.Count() > 0)
                            {

                                SendMail();
                                LastMailSendHour = DateTime.Now.Hour;

                            }

                            System.Threading.Thread.Sleep(1000 * 60);

                        }

                    }));

                    TaskManager.Start();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ErrorLogInformation : " + "\t" + ex.Message, null, false);
            }

        }

        public void Dispose()
        {
            SendMail();
            TaskManager.Abort();
        }

    }
    public class NotificationLog
    {
        public string DistributorName { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerOperatorCode { get; set; }
        public string CustomerOperatorName { get; set; }
        public string Imeı { get; set; }
        public string ConsolidationName { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string TableRow
        {
            get
            {

                return "<tr>" +
                    "<td>" + Date.ToString("yyyy-MM-dd HH:mm:ss") + "</td>" +
                    "<td>" + DistributorName + "</td>" +
                    "<td>" + InvoiceNumber + "</td>" +
                    "<td>" + CustomerOperatorCode + "</td>" +
                    "<td>" + CustomerOperatorName + "</td>" +
                    "<td>" + Imeı + "</td>" +
                    "<td>" + Message + "</td>" +
                    "</tr>";

            }
        }
    }
}
