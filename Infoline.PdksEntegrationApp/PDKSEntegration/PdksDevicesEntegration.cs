using Infoline.PdksEntegrationApp.Devices;
using Infoline.PdksEntegrationApp.Devices.Models;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp.PDKSEntegration
{
    public class PdksDevicesEntegration : IDisposable
    {
        public List<PdksDevice> devices = new List<PdksDevice>();
        List<Task> Tasks = new List<Task>();


        public PdksDevicesEntegration()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            var db = tenant.GetDatabase();

            var PdksDevices = db.GetSH_ShiftTrackingDevice();

            foreach (var device in PdksDevices)
            {
                if(device.DeviceBrand.ToLower() == "zkteco" && device.DeviceModel.ToLower() == "sf300")
                {
                    this.devices.Add(new ZKTecoSF300().B_EntityDataCopyForMaterial(device));
                }else if(device.DeviceBrand.ToLower() == "zkteco" && device.DeviceModel.ToLower() == "tk100-c")
                {
                    this.devices.Add(new ZKTecoTK100_C().B_EntityDataCopyForMaterial(device));
                }
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
