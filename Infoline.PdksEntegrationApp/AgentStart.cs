using Infoline.PdksEntegrationApp.ZKTEcoSF300Entegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp
{
    public class AgentStart : ServiceBase
    {
        List<Task> Tasks = new List<Task>();

        public AgentStart()
        {

        }

        public void Run()
        {

            var taskProcessZKTEcoSF300Entegration = new Task(() =>
            {
                new ProcessZKTEcoSF300Entegration().Run();
            });
            Tasks.Add(taskProcessZKTEcoSF300Entegration);

            taskProcessZKTEcoSF300Entegration.Start();
        }

        protected override void OnStart(string[] args)
        {
            Run();
        }

        protected override void Dispose(bool disposing)
        {
            Log.Success("Görevler dispose ediliyor");

            if (Tasks != null && Tasks.Count > 0)
            {
                foreach (var task in Tasks)
                {
                    task.Dispose();
                }
                Tasks = new List<Task>();
            }
            base.Dispose(disposing);
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
