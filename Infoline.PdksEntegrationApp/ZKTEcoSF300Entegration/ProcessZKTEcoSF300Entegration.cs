using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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

        public Task Run()
        {
            device.Connect();
            while (true)
            {
                if (device.isConnected())
                {
                    Log.Success("Cihaz ile bağlantı başarılı bir şekilde kuruldu.");
                    if (insertLogsToDB())
                    {
                        //device.ClearAllLog();
                    }
                    Thread.Sleep(600000); //10 dk uyu
                }
                else
                {
                    device.Connect();
                    device.unlockDevice();
                    device.RestartDevice();
                    if (!device.isConnected())
                    {
                        Log.Error("Cihaz ile bağlantı kurulamıyor.. Lütfen cihazın açık olduğundan emin olun ve bağlantılarını kontrol edin");
                        Thread.Sleep(60000); // 1dk uyu
                    }

                }
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
