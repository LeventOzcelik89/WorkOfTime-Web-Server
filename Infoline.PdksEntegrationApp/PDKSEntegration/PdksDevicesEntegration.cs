using Infoline.PdksEntegrationApp.Devices;
using Infoline.PdksEntegrationApp.Devices.Models;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Newtonsoft.Json;
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
            var serviceDomain = TenantConfig.Tenant.GetWebServiceUrl();
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();

#if DEBUG
            serviceDomain = "http://localhost:54598/";
#endif

            List<SH_ShiftTrackingDevice> PdksDevices = new List<SH_ShiftTrackingDevice>();
            using (var client = new RestClient(serviceDomain))
            {
                var request = new RestRequest("SH_ShiftTrackingDevice/GetAll", Method.Get);
                request.AddHeader("Cookie", "ASP.NET_SessionId=anq5tyqhj1yfbkmre0efr2kw");
                request.AddParameter("tenantCode", tenantCode);
                var response = client.ExecuteAsync(request).Result;
                PdksDevices = Infoline.Helper.Json.Deserialize<List<SH_ShiftTrackingDevice>>(response.Content);
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
            Log.Info("ProcessPDKSEntegration is Start");
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
