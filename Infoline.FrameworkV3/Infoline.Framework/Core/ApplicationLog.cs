using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infoline.Helper;
using System.Diagnostics;

namespace Infoline
{
    public class ApplicationLog
    {
       public static  string sSource="Infoline";
       public static string sLog = "Infoline-WEB";
        public static void LogError(Exception ex)
        {

            System.Diagnostics.Debug.WriteLine(ex.ToString());
            if (UnitTestDetector.IsInUnitTest)
                throw ex;
            else
                ((ILogListener)Application.Current).LogError(ex);       


        }
        public static void LogMessage(LogLevel level, string message)
        {
            if (UnitTestDetector.IsInUnitTest)
                System.Diagnostics.Debug.Write(message);
            else
            {
                if (System.Diagnostics.Debugger.IsAttached || Environment.UserInteractive)
                {
                    var msg = string.Format("{0} {2} {1} ", level, message, SW.Elapsed.Milliseconds);
                    System.Console.WriteLine(msg);
                    System.Diagnostics.Debug.WriteLine(msg);
                    SW.Restart();
                }
                ((ILogListener)Application.Current).LogMessage(level, message);
            } 

        }
        static System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();
        public static void LogError(string message)
        {
            LogMessage(LogLevel.Error, message);
        }

        public static void LogInfo(string message)
        {
            LogMessage(LogLevel.Info, message);
        }
    }
    public interface ILogListener
    {
        bool Enabled { get; }
        void LogMessage(LogLevel level, string message);
        void LogError(Exception ex);
    }
    public enum LogLevel
    {
        Verbose,
        UserError,
        UserWarning,
        UserInfo,
        Debug,
        Info,
        Warning,
        Error,
    }
}
