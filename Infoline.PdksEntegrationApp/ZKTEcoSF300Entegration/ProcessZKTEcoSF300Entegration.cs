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
        public List<ZKTecoSF300> devices = new List<ZKTecoSF300>();
        List<Task> Tasks = new List<Task>();


        public ProcessZKTEcoSF300Entegration()
        {
            var db = new WorkOfTimeDatabase();

            var PdksDevices = db.GetSH_ShiftTrackingDeviceByBrandAndModel("ZKTEco", "SF300");

            foreach (var device in PdksDevices)
            {
                this.devices.Add(new ZKTecoSF300().B_EntityDataCopyForMaterial(device));
            }

            Log.Info(devices.Count() + " Cihaz Bulundu");
            Log.Info("ProcessTitanEntegration is Start");
        }

        public void Run()
        {

            foreach (var device in devices)
            {
                var deviceThread = new Task(() =>
                {
                    device.Run();
                });
                Tasks.Add(deviceThread);
                deviceThread.Start();

            }

        }

     
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
