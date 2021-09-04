using System;
using System.IO;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public static class Log
    {

        public delegate void RestartEventHandler();
        public static event RestartEventHandler RestartEvent;
        public delegate void StopEventHandler();
        public static event StopEventHandler StopEvent;
        static bool _start = true;
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
        static bool _reset = false;
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


        private static void WriteTime()
        {
            WriteTime(false);
        }
        static object sync = new object();
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
                message = ("Info : " + message);
                Console.ForegroundColor = ConsoleColor.Gray;
                try
                {
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                    Console.WriteLine(message);
                    Save(message);
                }
            }
        }


        public static void Success(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime();


                message = ("Success : " + message);
                Console.ForegroundColor = ConsoleColor.Green;
                try
                {
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                    Console.WriteLine(message);
                    Save(message);
                }

            }
        }

        public static void Error(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                message = ("Error : " + message);
                Console.ForegroundColor = ConsoleColor.Red;
                try
                {
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                    Console.WriteLine(message);
                    Save(message);
                }
            }
        }

        public static void Warning(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                message = ("Warning : " + message);
                Console.ForegroundColor = ConsoleColor.Red;
                try
                {
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                    Console.WriteLine(message);
                    Save(message);
                }
            }
        }

        static string FILE = "";
        private static void Save(string str)
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
            if (string.IsNullOrEmpty(FILE)) FILE = path + string.Format(@"\{0}-log.txt", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));


            using (StreamWriter sw = File.AppendText(FILE))
            {
                sw.WriteLine(DateTime.Now + ":" + str);
            }


        }

    }
}