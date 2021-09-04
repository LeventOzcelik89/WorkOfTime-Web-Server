using Infoline.WorkOfTime.BusinessAccess;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Linq;


namespace DvrAlertSnmpAgent
{
    public partial class ServiceStart : ServiceBase
    {
        List<TEN_Tenant> Tenants = new List<TEN_Tenant>();
        List<Task> Tasks = new List<Task>();

        public ServiceStart()
        {
            Tenants = TenantConfig.GetTenants();
        }

        public void Process()
        {
            var tenant = Tenants.Where(a => a.TenantCode == 1100).FirstOrDefault();
            var t = new Task(() =>
            {
                new IOTCameraLogger(tenant);
            });

            Tasks.Add(t);
            t.Start();
        }

        protected override void Dispose(bool disposing)
        {
            Log.Success("Dispose");

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

        protected override void OnStart(string[] args)
        {
            Log.Success("OnStart()");
            Process();
        }

        protected override void OnStop()
        {
            Log.Success("OnStop()");
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