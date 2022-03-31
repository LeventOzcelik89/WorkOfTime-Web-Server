using Infoline.PdksEntegrationApp.Devices;
using Infoline.PdksEntegrationApp.Devices.Models;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            var serviceDomain = ConfigurationManager.AppSettings["DefaultWebServiceUrl"].ToString();

            List<SH_ShiftTrackingDevice> PdksDevices = new List<SH_ShiftTrackingDevice>();
            using (var client = new RestClient(serviceDomain))
            {
                var request = new RestRequest("SH_ShiftTrackingDevice/GetAll", Method.Get);
                var response = client.ExecuteAsync(request).Result;
                try
                {
                    PdksDevices = JsonSerializer.Deserialize<List<SH_ShiftTrackingDevice>>(response.Content);
                }
                catch (Exception ex)
                {
                    Log.Info("Cihaz Listesi Alınamadı" + ex.Message);
                }
            }

            foreach (var device in PdksDevices)
            {
                if (device.DeviceBrand.ToLower() == "zkteco" && device.DeviceModel.ToLower() == "sf300")
                {
                    this.devices.Add(new ZKTecoSF300().B_EntityDataCopyForMaterial(device));
                }
                else if (device.DeviceBrand.ToLower() == "zkteco" && device.DeviceModel.ToLower() == "tk100-c")
                {
                    this.devices.Add(new ZKTecoTK100_C().B_EntityDataCopyForMaterial(device));
                }
                else if (device.DeviceBrand.ToLower() == "zkteco" && device.DeviceModel.ToLower() == "k70")
                {
                    this.devices.Add(new ZKTecoK70().B_EntityDataCopyForMaterial(device));
                }
            }

            Log.Info(devices.Count() + " Cihaz Bulundu");
            Log.Info("PDKS Entegrasyonu Başladı");
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
