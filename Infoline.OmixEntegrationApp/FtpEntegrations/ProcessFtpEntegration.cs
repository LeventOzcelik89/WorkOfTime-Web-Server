using Infoline.OmixEntegrationApp.FtpEntegrations.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegrations
{

    public class ProcessFtpEntegration : IDisposable
    {
        List<Task> Tasks = new List<Task>();
        public ProcessFtpEntegration()
        {
            Log.Info("ProcessFtpEntegration is Start");
        }
        public void Run()
        {
            Log.Error("test");
            var taskMobitel = new Task(() =>
            {
                new FtpMobitel().ExportFilesToDatabase(); ;
            });
            Tasks.Add(taskMobitel);

            var taskGenpa = new Task(() =>
            {
                new FtpGenpa().ExportFilesToDatabase(); ;
            });
            Tasks.Add(taskGenpa);

            var taskKvk = new Task(() =>
            {
                new FtpKvk().ExportFilesToDatabase(); ;
            });
            Tasks.Add(taskKvk);

            var taskPort = new Task(() =>
            {
                new FtpPort().ExportFilesToDatabase(); ;
            });
            Tasks.Add(taskPort);

            foreach (var task in Tasks)
            {
                task.Start();
            }

        }
        public void Dispose()
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
