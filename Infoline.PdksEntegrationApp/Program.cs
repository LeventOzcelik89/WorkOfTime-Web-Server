using ServiceInstaller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AgentRunControl(); //Çalışan Bir Uygulama Var İse Kapatalım

            //Windows Servis olarak mı çalışıyor
            if (!Environment.UserInteractive) //windows Servis 
            {
                System.ServiceProcess.ServiceBase.Run(new System.ServiceProcess.ServiceBase[] { new AgentStart() });
            }
            else //Windows Console şeklinde çalışır ise
            {
                try
                {
                    ServiceControl(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Servis Yüklenirken Hata Oluştu..." + Environment.NewLine + ex.Message.ToString());
                }

                try
                {
                    //Örnek Kullanım DB Connection İçin
                    //var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
                    //var db = new WorkOfTimeDatabase();
                    //var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
                    //db = tenant.GetDatabase();
                    //var d = db.GetSH_User();

                    Console.WriteLine("Agent is Start...");
                    var agentStart = new AgentStart();
                    agentStart.Run();
                    while (true)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now + ":Hata" + ex.Message);
                }

            }
        }


        private static void ServiceControl(string[] args)
        {
            var ServiceName = "Infoline.PdksEntegrationApp";
            if (SvcInstaller.GetServiceStatus(ServiceName) == ServiceState.NotFound) // Windows Servisi Kur Yüklü Değil ise
            {
                Console.WriteLine("Servis Kuruluyor");

                SvcInstaller.ServisInstall(ServiceName, false, false, System.Reflection.Assembly.GetEntryAssembly().Location);
            }
            if (args.Count() > 0 && args[0] == "u")
            {
                SvcInstaller.ServisUnInstall(ServiceName);
            }
            SvcInstaller.ServisStop(ServiceName);
        }

        private static void AgentRunControl()
        {

            if (Environment.UserInteractive)
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals("Infoline.PdksEntegrationApp"));
                if (sc != null)
                {
                    if (sc.Status.Equals(ServiceControllerStatus.Running))
                    {
                        Console.WriteLine("Çalışan bir Servis Agent var...! Uygulama kapatılıyor... {0} {1} {2}", sc.CanStop, sc.CanShutdown, sc.CanPauseAndContinue);
                        sc.Stop();
                        sc.Dispose();
                        Thread.Sleep(5 * 1000);
                    }
                }
            }

            if (Process.GetProcessesByName("Infoline.PdksEntegrationApp").Length > 1)
            {
                Console.WriteLine("Çalışan bir Agent var...! Diğer Uygulama kapatılıyor...");

                var runningProcesses = Process.GetProcesses().Where(a => a.ProcessName.Equals("Infoline.PdksEntegrationApp"));
                foreach (var process in runningProcesses)
                {
                    if (System.IntPtr.Zero != process.MainWindowHandle && Process.GetCurrentProcess().MainWindowHandle != process.MainWindowHandle)
                    {
                        Console.WriteLine(process.ProcessName + " (Kill-2)");
                        process.Kill();
                    }
                }
                Thread.Sleep(2 * 1000);
            }
        }
    }
}
