using Infoline.Framework.ServiceInstaller;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvrAlertSnmpAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities.ConfigureSQL();
            Log.Success("Ajan Başlatıldı.");

            if (System.Diagnostics.Process.GetProcessesByName("Infoline.WorkOfTime.DvrAlertSnmpAgent").Length > 1)
            {
                Console.WriteLine("Çalışan bir Infoline.WorkOfTime.DvrAlertSnmpAgent  var...! , Uygulama kapatılıyor...");
                System.Threading.Thread.Sleep(2 * 1000);
                Environment.Exit(0);
                return;
            }

            var ServiceName = "Infoline.WorkOfTime.DvrAlertSnmpAgent";
            if (!Environment.UserInteractive)
            {
                System.ServiceProcess.ServiceBase.Run(new System.ServiceProcess.ServiceBase[] { new ServiceStart() });
            }

            else
            {
                try
                {
                    if (SvcInstaller.GetServiceStatus(ServiceName) == ServiceState.NotFound)
                    {
                        SvcInstaller.ServisInstall(ServiceName, false, false, System.Reflection.Assembly.GetEntryAssembly().Location);
                    }

                    if (args.Count() > 0 && args[0] == "u")
                    {
                        SvcInstaller.ServisUnInstall(ServiceName);
                    }

                    SvcInstaller.ServisStop(ServiceName);
                }
                catch
                {

                }

                var agentStart = new ServiceStart();
                agentStart.Process();
                do
                {
                    System.Threading.Thread.Sleep(1000);
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                agentStart.Stop();
            }
        }
    }
}
