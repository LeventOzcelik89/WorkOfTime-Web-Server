using Infoline.OmixEntegrationApp.FtpEntegrations;
using Infoline.OmixEntegrationApp.LogoEntegration;
using Infoline.OmixEntegrationApp.TitanEntegration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp
{
    public class AgentStart : ServiceBase
    {
        List<Task> Tasks = new List<Task>();

        public AgentStart()
        {

        }

        public void Run()
        {
            Log.Error("TEST");
            var taskProcessLogoEntegration = new Task(() =>
            {
                new ProcessLogoEntegration().Run();
            });
            Tasks.Add(taskProcessLogoEntegration);

            var taskProcessTitanEntegration = new Task(() =>
            {
                new ProcessTitanEntegration().Run();
            });
            Tasks.Add(taskProcessTitanEntegration);

            var taskProcessFtpDistEntegration = new Task(() =>
            {
                new ProcessFtpEntegration().Run();
            });
            Tasks.Add(taskProcessFtpDistEntegration);

            //taskProcessLogoEntegration.Start();
            taskProcessFtpDistEntegration.Start();
            taskProcessTitanEntegration.Start();

        }

        protected override void OnStart(string[] args)
        {
            //var startingDate= ConfigurationManager.AppSettings["WorkerStart"].ToString();
            //if (string.IsNullOrEmpty(startingDate))
            //{
            //    Log.Error("Ajan çalışma saati bulunamadı");
            //    throw new Exception("Ajan çalışma saati bulunamadı");
            //}
            //var parseDate = TimeSpan.ParseExact(startingDate,"HH:mm",CultureInfo.InvariantCulture);


            //while (true)
            //{
            //    var getDate = DateTime.Now;

            //    if (getDate==new DateTime(getDate.Year,getDate.Month,getDate.Day,0,30,getDate.Second))
            //    {
            //        Run();
            //    }
            //    Thread.Sleep(new TimeSpan(0,1,0));
            //}
            Run();
          
        }

        protected override void OnStop()
        {
            Log.Success("Tasklar durduruldu.");
            if (Tasks != null && Tasks.Count > 0)
            {
                foreach (var task in Tasks)
                {
                    task.Dispose();
                }
                Tasks = new List<Task>();
            }
        }
    }
}
