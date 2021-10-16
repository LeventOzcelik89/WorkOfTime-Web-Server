using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.TitanEntegration.Models;
using Infoline.WorkOfTime.BusinessAccess;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using System.Configuration;

namespace Infoline.OmixEntegrationApp.TitanEntegration.Business
{
    public class TitanService
    {
        private WorkOfTimeDatabase db = new WorkOfTimeDatabase();
        public string Host { get => ConfigurationManager.AppSettings["Host"].ToString(); }
        public TitanService()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            db = tenant.GetDatabase();
        }

        public void CompensateFromTitanServices()
        {
            Log.Info("Titan Services Compenstate is Start...");
            var getAllDevices = GetLastDeviceListFromTitanServices();
            if (getAllDevices.objects != null)
            {
                var getAllDevicesList = (DeviceResultList)getAllDevices.objects;
                var databaseDevices = db.GetPRD_TitanDeviceActivated().ToList();
                var savingList = getAllDevicesList.Data.Where(x => !databaseDevices.Select(a => a.IMEI1).Contains(x.IMEI1) || !databaseDevices.Select(a => a.IMEI2).Contains(x.IMEI2) || !databaseDevices.Select(a => a.SerialNumber).Contains(x.Serial));
                Log.Info("New Activated Device Count : {0}", savingList.Count());

                var resut = db.BulkInsertPRD_TitanDeviceActivated(savingList.Select(x => new PRD_TitanDeviceActivated
                {
                    CreatedOfTitan = x.Created,
                    DeviceId = new Guid(x.DeviceId),
                    IMEI1 = x.IMEI1,
                    IMEI2 = x.IMEI2,
                    InventoryId = db.GetPRD_InventoryBySerialCodeOrImei(x.Serial, x.IMEI1, x.IMEI2)?.id,
                    ProductId = db.GetPRD_InventoryBySerialCodeOrImei(x.Serial, x.IMEI1, x.IMEI2)?.productId,
                    SerialNumber = x.Serial,
                    TitanDeviceName = x.DeviceName,
                    TitanModel = x.Model,
                    TitanProduct = x.Product
                }));
            }
            Log.Info("Titan Services Compenstate End...");
        }

        public void CompensateFromInventory()
        {
            //DB den Eşleşmemişleri bul
            //Eşleşmemişleri tekrar karşılaştır Envanter ID si bulunanları Update et
        }

        private ResultStatus GetLastDeviceListFromTitanServices()
        {
            var lastDataTime = db.GetPRD_TitanDeviceActivatedGetAllLastDate();
            Log.Info("Titan Services last connected time : {0}", lastDataTime);

            var query = Helper.Json.Serialize(new
            {
                DataType = 0,
                Start = lastDataTime,
                End = DateTime.Now
            });
            return GetDeviceListFromTitanServices<DeviceResultList>(ConfigurationManager.AppSettings["GetDeviceActivationInformation"].ToString(), query);
        }

        private ResultStatus GetDeviceListFromTitanServices<T>(string uri, string query = null)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = WebRequest.Create(Host + uri);
                request.ContentType = "application/json";
                request.Method = "GET";
                var type = request.GetType();
                var currentMethod = type.GetProperty("CurrentMethod", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(request);
                var methodType = currentMethod.GetType();
                methodType.GetField("ContentBodyNotAllowed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentMethod, false);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(query);
                }
                var response = (HttpWebResponse)(request.GetResponse());
                using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    return new ResultStatus
                    {
                        result = true,
                        message = "istek başarılı",
                        objects = JsonConvert.DeserializeObject<T>(reader.ReadToEnd())
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResultStatus
                {
                    result = false,
                    message = ex.Message,
                    objects = null
                };
            }
        }
    }
}
