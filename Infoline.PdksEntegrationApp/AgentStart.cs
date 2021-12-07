using Infoline.PdksEntegrationApp.PDKSEntegration;
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

            var taskProcessPdksDevicesEntegration = new Task(() =>
            {
                new PdksDevicesEntegration().Run();
            });
            Tasks.Add(taskProcessPdksDevicesEntegration);

            taskProcessPdksDevicesEntegration.Start();
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
