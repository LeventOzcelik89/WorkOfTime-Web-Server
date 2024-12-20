﻿using Infoline.OmixEntegrationApp.FtpEntegrations;
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
            taskProcessTitanEntegration.Start();
            taskProcessFtpDistEntegration.Start();
        }

        protected override void OnStart(string[] args)
        {
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
