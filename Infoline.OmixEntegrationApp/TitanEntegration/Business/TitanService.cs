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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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


                foreach (var item in savingList)
                {
                    var checkDb = db.GetVWPRD_TitanDeviceActivatedByImei(item.IMEI1 == null ? item.Serial : item.IMEI1);
                    if (checkDb == null)
                    {
                        var prdTitanDeviceActivated = new PRD_TitanDeviceActivated();

                        var getInventory = db.GetPRD_InventoryBySerialCodeOrImei(item.Serial, item.IMEI1, item.IMEI2);
                        prdTitanDeviceActivated.CreatedOfTitan = item.Created.ToLocalTime();
                        prdTitanDeviceActivated.DeviceId = new Guid(item.DeviceId);
                        prdTitanDeviceActivated.IMEI1 = item.IMEI1 == null ? item.Serial : item.IMEI1;
                        prdTitanDeviceActivated.IMEI2 = item.IMEI2;
                        prdTitanDeviceActivated.InventoryId = getInventory?.id;
                        prdTitanDeviceActivated.ProductId = getInventory?.productId;
                        prdTitanDeviceActivated.SerialNumber = item.Serial;
                        prdTitanDeviceActivated.TitanDeviceName = item.DeviceName;
                        prdTitanDeviceActivated.TitanModel = item.Model;
                        prdTitanDeviceActivated.TitanProduct = item.Product;

                        var dbInsertResult = db.InsertPRD_TitanDeviceActivated(prdTitanDeviceActivated);

                        if (!dbInsertResult.result)
                            Log.Error("IMEI is {0} not saving for db", item.IMEI1);
                    }
                    else
                    {
                        Log.Warning("IMEI is {0} already exist...", item.IMEI1);
                    }
                }

            }
            Log.Info("Titan Services Compenstate End...");
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

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Host + uri);
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

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                        | SecurityProtocolType.Tls11
                                                        | SecurityProtocolType.Tls12
                                                        | SecurityProtocolType.Ssl3
                                                        | SecurityProtocolType.SystemDefault
                                                        | (SecurityProtocolType)12288;

                ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;

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
                Log.Error("Servis Çağırılırken Hata Alındı : {0} ", ex.Message);

                return new ResultStatus
                {
                    result = false,
                    message = ex.Message,
                    objects = null
                };
            }
        }
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            else
            {
                var requestCertificate = (X509Certificate2)certificate;
                var logEntry = new StringBuilder();
                logEntry.AppendFormat("SSL Policy Error(s): {0} - Cert Issuer: {1} - SubjectName: {2}",
                   sslPolicyErrors.ToString(),
                   requestCertificate.Issuer,
                   requestCertificate.SubjectName.Name);
                if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors) //Root CA problem
                {
                    if (chain != null && chain.ChainStatus != null)
                    {
                        foreach (var chainStatus in chain.ChainStatus)
                        {
                            logEntry.AppendFormat("|Chain Status: {0} - {1}", chainStatus.Status.ToString(), chainStatus.StatusInformation.Trim());
                        }
                    }
                }
                return false;
            }
        }
    }
}
