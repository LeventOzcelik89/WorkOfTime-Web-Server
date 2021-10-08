using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp
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
                        Log.Warning("Reset");
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
        public static void Success2(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime();
                try
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
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
            //  lock (sync)
            {
                WriteTime(true);
                try
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();

                    Save("ERR:" + string.Format(message, parameters));

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
                }
                catch
                {
                }
                Save(string.Format(message, parameters));
            }
        }
        public static void Warning2(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();
                }
                catch
                {
                }
                Save(string.Format(message, parameters));
            }
        }
        public static void Warning3(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();
                }
                catch
                {
                }
                Save(string.Format(message, parameters));
            }
        }
        public static void Warning4(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();
                }
                catch
                {
                }
                Save(string.Format(message, parameters));
            }
        }
        public static void Exception(Exception ex, string message, params object[] parameters)
        {
            lock (sync)
            {

                if (ex is System.Threading.ThreadAbortException)
                    return;
                Error(message);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("ERROR : " + ex.Message);
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    Save(string.Format(message, parameters));

                }
                catch
                {
                }
            }
        }

        static string FILE = "";

        private static void Save(string str)
        {

            try
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Log");
                if (string.IsNullOrEmpty(str)) return;
                if (string.IsNullOrEmpty(FILE)) FILE = AppDomain.CurrentDomain.BaseDirectory + @"Log" + Path.DirectorySeparatorChar + string.Format(@"{0}-log.txt", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));

                if (!File.Exists(FILE)) System.IO.File.WriteAllText(FILE, DateTime.Now.ToString());

                using (StreamWriter sw = File.AppendText(FILE))
                {
                    sw.WriteLine(DateTime.Now + ":" + str);
                }

            }
            catch (Exception)
            {

            }

        }

    }
}
