using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class TitanServices
    {
        public string Host { get => ConfigurationManager.AppSettings["Host"].ToString(); }
        private ResultStatus SendRequest<T>(string uri, string query = null)
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
                var response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
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
                    message = ex.Message
                };
            }
        }
        public ResultStatus GetAllDevices()
        {
            return SendRequest<DeviceResultList>(ConfigurationManager.AppSettings["Getlist"].ToString());
        }
        public ResultStatus GetDeviceById(Guid id)
        {
            return SendRequest<DeviceResult>(ConfigurationManager.AppSettings["Getbyid"].ToString() + id);
        }
        public ResultStatus GetDeviceInformation(Guid id)
        {
            return SendRequest<DeviceResult>(ConfigurationManager.AppSettings["Getdeviceinformation"].ToString() + id);
        }
        public ResultStatus GetDeviceActivationInformations()
        {
            var query = Helper.Json.Serialize(new
            {
                DataType = 0,
                Start = DateTime.Now.AddYears(-20),
                End = DateTime.Now
            });
            return SendRequest<DeviceResultList>(ConfigurationManager.AppSettings["GetDeviceActivationInformation"].ToString(), query);
        }
    }
    public class DeviceApplication
    {
        public string ApplicationId { get; set; }
        public string UrlScheme { get; set; }
        public string Name { get; set; }
        public string PackageName { get; set; }
        public string UniqueName { get; set; }
        public string Version { get; set; }
        public string ActivityName { get; set; }
        public bool IsSystemApp { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceData
    {
        public string Board { get; set; }
        public string Brand { get; set; }
        public string DeviceName { get; set; }
        public string Hardware { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Product { get; set; }
        public string Serial { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public DeviceOperatingSystem OperatingSystem { get; set; } = new DeviceOperatingSystem();
        public object HardwareDetail { get; set; }
        public List<object> GsmCarriers { get; set; }
        public DeviceLastUsageHistory LastUsageHistory { get; set; } = new DeviceLastUsageHistory();
        public DeviceLastLocation LastLocation { get; set; } = new DeviceLastLocation();
        public List<DeviceApplication> Applications { get; set; } = new List<DeviceApplication>();
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceLastLocation
    {
        public object Country { get; set; }
        public object Region { get; set; }
        public object County { get; set; }
        public string LocationId { get; set; }
        public string Longitude { get; set; } = "0";
        public string Latitude { get; set; } = "0";
        public double Altitude { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Created { get; set; }
        public object Modified { get; set; }
        public string DeviceId { get; set; }
        public IGeometry Location { get; set; }
    }
    public class DeviceLastUsageHistory
    {
        public string UsageHistoryId { get; set; }
        public double? Battery { get; set; }
        public double? Storage { get; set; }
        public double? CPU { get; set; }
        public double? RAM { get; set; }
        public bool? Broken { get; set; } = false;
        public bool? ForcedBreak { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceOperatingSystem
    {
        public string OperatingSystemId { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceResult
    {
        public DeviceData Data { get; set; }
        public bool? Success { get; set; }
        public List<string> Messages { get; set; }
    }
    public class DeviceResultList
    {
        public List<DeviceData> Data { get; set; }
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
    }
}
