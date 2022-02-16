using Infoline.Framework.ServiceInstaller;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Linq;

namespace Infoline.WorkOfTime.Agent
{
	static class Program
	{
		static void Main(string[] args)
		{


			Utilities.ConfigureSQL();
			Log.Success("Ajan Başlatıldı.");
			if (System.Diagnostics.Process.GetProcessesByName("Infoline.WorkOfTime.Agent").Length > 1)
			{
				Console.WriteLine("Çalışan bir Infoline.WorkOfTime.Agent  var...! , Uygulama kapatılıyor...");
				System.Threading.Thread.Sleep(2 * 1000);
				Environment.Exit(0);
				return;
			}
			var ServiceName = "Infoline.WorkOfTime.Agent";
			if (!Environment.UserInteractive) //windows Servis 
			{
				System.ServiceProcess.ServiceBase.Run(new System.ServiceProcess.ServiceBase[] { new ServiceStart() });
			}
			else
			{
				try
				{
					if (SvcInstaller.GetServiceStatus(ServiceName) == ServiceState.NotFound) // Otomatik servisi Kur
					{
						SvcInstaller.ServisInstall(ServiceName, false, false, System.Reflection.Assembly.GetEntryAssembly().Location);
					}

					if (args.Any() && args[0] == "u")
					{
						SvcInstaller.ServisUnInstall(ServiceName);
					}

					SvcInstaller.ServisStop(ServiceName);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Agentda hata oluştu.Mesaj:" + ex.Message);

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
