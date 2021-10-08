using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.TitanEntegration.Models;
using Infoline.WorkOfTime.BusinessAccess;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using System.Data.Common;
using System.Configuration;

namespace Infoline.OmixEntegrationApp.TitanEntegration.Business
{
    public class TitanService : ITitanService
    {
        public string Host { get => ConfigurationManager.AppSettings["Host"].ToString(); }
        public WorkOfTimeDatabase Db { get; set; }
       

        public  ResultStatus Sender<T>(string uri, string query = null)
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
        public void SaveAll()
        {

            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            Db = tenant.GetDatabase();
            var getAllDevices =  GetAll();
            var getlAllDevicesList = (DeviceResultList)getAllDevices.objects;
            var databaseDevices= Db.GetPRD_TitanDeviceActivated().ToList();
            var savingList=getlAllDevicesList.Data.Where(x => !databaseDevices.Select(a => a.IMEI1).Contains(x.IMEI1)|| !databaseDevices.Select(a => a.IMEI2).Contains(x.IMEI2)|| !databaseDevices.Select(a => a.SerialNumber).Contains(x.Serial));
            var resut=Db.BulkInsertPRD_TitanDeviceActivated(savingList.Select(x=>new PRD_TitanDeviceActivated {
                CreatedOfTitan = x.Created,
                DeviceId = new Guid(x.DeviceId),
                IMEI1 = x.IMEI1,
                IMEI2 = x.IMEI2,
                InventoryId = Db.GetPRD_InventoryBySerialCodeOrImei(x.Serial, x.IMEI1, x.IMEI2)?.id,
                ProductId = Db.GetPRD_InventoryBySerialCodeOrImei(x.Serial, x.IMEI1, x.IMEI2)?.productId,
                SerialNumber = x.Serial
            }));
        }
        public  ResultStatus GetAll()
        {
            var query = Helper.Json.Serialize(new
            {
                DataType = 0,
                Start = DateTime.Now.AddYears(-20),
                End = DateTime.Now
            });
            return Sender<DeviceResultList>("/Devices/GetDeviceActivationInformation", query);
        }
    }
}
