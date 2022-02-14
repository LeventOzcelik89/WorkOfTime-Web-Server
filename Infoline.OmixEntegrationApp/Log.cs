using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Infoline.OmixEntegrationApp
{

    public sealed class ErrorLogInformation : IDisposable
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

                if (System.Configuration.ConfigurationManager.AppSettings["LogMailUsers"] != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["LogMailUsers"].ToString();
                }

                return "seyit.tekce@infoline-tr.com";

            }
        }

        private static int? LastMailSendHour { get; set; }

        private static List<ErrorInformation> Errors { get; set; }

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
                    "<th width=\"200\">" + "Kullanıcı" + "</th>" +
                    "<th>" + "Mesaj" + "</th>" +
                    "<th>" + "Url" + "</th>" +
                    "<th>" + "IP" + "</th>" +
                    "<th>" + "Parametreler" + "</th>" +
                    "</tr>" +
                    "</thead>";

                tbl += "<tbody>" + String.Join("", Errors.Select(a => a.TableRow)) + "</tbody>" +
                    "</table>";

                new Email().Send(300, LogMailUsers, header, header + tbl, true, false);

                Errors.Clear();
            }
            catch (Exception ex) { }

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

        public static void SaveError(DateTime date, string message)
        {

            try
            {

                if (Errors == null)
                {
                    Errors = new List<ErrorInformation>();
                }

#if !DEBUG

                var userStatus = HttpContext.Current != null && HttpContext.Current.Session != null
                    ? (PageSecurity)HttpContext.Current.Session["userStatus"]
                    : null;

                Errors.Add(new ErrorInformation
                {
                    Message = message,
                    UserName = userStatus != null ? userStatus.user.loginname : "",
                    Date = date,
                    Url = HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "",
                    IP = HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.UserHostAddress : "",
                    Params = GetKeyValues()
                });

#endif

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

                            if (LastMailSendHour < DateTime.Now.Hour && Errors.Count() > 0)
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

    public class ErrorInformation
    {
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }
        public string Params { get; set; }
        public string TableRow
        {
            get
            {

                return "<tr>" +
                    "<td>" + Date.ToString("yyyy-MM-dd HH:mm:ss") + "</td>" +
                    "<td>" + UserName + "</td>" +
                    "<td>" + Message + "</td>" +
                    "<td>" + Url + "</td>" +
                    "<td>" + IP + "</td>" +
                    "<td>" + Params + "</td>" +
                    "</tr>";

            }
        }
    }

    public enum LogMessageType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3,
    }

    public class Log
    {
        public delegate void RestartEventHandler();
        public static event RestartEventHandler RestartEvent;
        public delegate void StopEventHandler();
        public static event StopEventHandler StopEvent;

        static string FILE = "";
        static string FILEERR = "";
        static bool _start = true;
        static bool _reset = false;
        static object sync = new object();

        public static bool START
        {
            get { return _start; }
            set
            {
                _start = value;
                if (!_start)
                    if (StopEvent != null)
                        StopEvent();
            }
        }

        public static bool RESET
        {
            get { return _reset; }
            set
            {
                _reset = value;
                if (_reset)
                {
                    _start = false;
                    if (RestartEvent != null)
                    {
                        RestartEvent();
                        Warning("Reset");
                        FILE = "";
                    }
                }
            }
        }

        private static Dictionary<LogMessageType, ConsoleColor> ConsoleColors
        {
            get
            {
                return new Dictionary<LogMessageType, ConsoleColor>
                {
                    { LogMessageType.Info, ConsoleColor.Gray },
                    { LogMessageType.Success, ConsoleColor.Green },
                    { LogMessageType.Warning, ConsoleColor.Yellow },
                    { LogMessageType.Error, ConsoleColor.Red },
                };
            }
        }

        private static void WriteTime()
        {
            WriteTime(false);
        }

        private static void WriteTime(bool errorconsole)
        {
            lock (sync)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (errorconsole)
                    {
                        Console.Error.Write(DateTime.Now.ToString().PadRight(20) + " ");
                    }
                    else
                    {
                        Console.Write(DateTime.Now.ToString().PadRight(20) + " ");
                    }
                    Console.ResetColor();
                }
                catch
                {
                }
            }
        }

        public static void Info(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime();
                try
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                }
            }
        }

        public static void Success(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime();
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));
                }
                catch
                {
                }
            }
        }

        public static void Error(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();

                    SaveERR(string.Format(message, parameters));

                }
                catch (Exception ex)
                {
                }
            }
        }

        public static void ConsoleMessage(LogMessageType type, string message)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(string.Format(message, ConsoleColors[type]));
                    Console.ResetColor();
                }
                catch
                {
                }
            }
        }

        public static void Warning(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();
                    Save(string.Format(message, parameters));

                }
                catch
                {
                }
            }
        }

        public static void Exception(Exception ex, string message, params object[] parameters)
        {
            lock (sync)
            {

                if (ex is System.Threading.ThreadAbortException)
                {
                    return;
                }

                Error(message);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("ERROR : " + ex.Message);
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    Save(string.Format(message, parameters));

                }
                catch { }
            }
        }

        private static void Save(string str)
        {

            if (string.IsNullOrEmpty(str)) { return; }

            var path = "";
            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Server.MapPath("\\log");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "log";
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (string.IsNullOrEmpty(str)) return;
            if (string.IsNullOrEmpty(FILE)) FILE = path + string.Format(@"\{0}-log.txt", DateTime.Now.ToString("yyyy-MM-dd"));

            using (StreamWriter sw = File.AppendText(FILE))
            {
                sw.WriteLine(DateTime.Now + ":" + "\t" + str);
            }

        }

        private static void SaveERR(string str)
        {
            ErrorLogInformation.SaveError(DateTime.Now, str);

            var path = "";
            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Server.MapPath("\\log");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "log";
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (string.IsNullOrEmpty(str)) return;
            if (string.IsNullOrEmpty(FILEERR)) FILEERR = path + string.Format(@"\{0}-logERR.txt", DateTime.Now.ToString("yyyy-MM-dd"));

            using (StreamWriter sw = File.AppendText(FILEERR))
            {
                sw.WriteLine(DateTime.Now + ":" + "\t" + str + System.Environment.NewLine + ErrorLogInformation.GetKeyValues());
            }
        }
    }
}
