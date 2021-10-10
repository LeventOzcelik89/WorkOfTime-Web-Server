using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp.ZKTEcoSF300Entegration
{
    class ProcessZKTEcoSF300Entegration : IDisposable
    {
        ZKTecoSF300 device { get; set; }
        private WorkOfTimeDatabase db = new WorkOfTimeDatabase();

        public ProcessZKTEcoSF300Entegration()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            db = tenant.GetDatabase();

            //ip port makine numarası bilgileri dbden gelicek
            device = new ZKTecoSF300("192.168.1.205",1,4370);
            Log.Info("ProcessTitanEntegration is Start");
        }

        public  Task Run()
        {
            while (true)
            {

                if (insertLogsToDB())
                {
                    device.ClearAllLog();
                }
                Task.Delay(new TimeSpan(0, 10, 0));
            }
        }

        private bool insertLogsToDB()
        {
            var logs = device.GetLogData();


            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
