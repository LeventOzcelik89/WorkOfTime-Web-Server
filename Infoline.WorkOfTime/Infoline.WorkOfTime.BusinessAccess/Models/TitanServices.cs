using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class DeviceApplication
    {
        public string ApplicationId { get; set; }
        public object UrlScheme { get; set; }
        public string Name { get; set; }
        public string PackageName { get; set; }
        public object UniqueName { get; set; }
        public string Version { get; set; }
        public string ActivityName { get; set; }
        public bool IsSystemApp { get; set; }
        public DateTime Created { get; set; }
        public object Modified { get; set; }
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
        public DeviceOperatingSystem OperatingSystem { get; set; }
        public object HardwareDetail { get; set; }
        public List<object> GsmCarriers { get; set; }
        public DeviceLastUsageHistory LastUsageHistory { get; set; }
        public DeviceLastLocation LastLocation { get; set; }
        public List<DeviceApplication> Applications { get; set; }
        public DateTime Created { get; set; }
        public object Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceLastLocation
    {
        public object Country { get; set; }
        public object Region { get; set; }
        public object County { get; set; }
        public string LocationId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public double Altitude { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public object Modified { get; set; }
        public string DeviceId { get; set; }
        public IGeometry Location { get; set; }
    }
    public class DeviceLastUsageHistory
    {
        public string UsageHistoryId { get; set; }
        public double Battery { get; set; }
        public double Storage { get; set; }
        public double CPU { get; set; }
        public double RAM { get; set; }
        public bool Broken { get; set; }
        public bool ForcedBreak { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public object Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceOperatingSystem
    {
        public string OperatingSystemId { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
        public DateTime Created { get; set; }
        public object Modified { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceResult
    {
        public DeviceData Data { get; set; }
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
    }
    public class DeviceResultList
    {
        public List<DeviceData> Data { get; set; }
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
    }
    public class TitanServices
    {
        private string Host { get { return "https://titantest.infoline-tr.com/api/v2"; } }
        private ResultStatus SendRequest<T>(string uri, string query = null)
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
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                return new ResultStatus
                {
                    result = true,
                    message = "istek başarılı",
                    objects = JsonConvert.DeserializeObject<T>(reader.ReadToEnd())
                };
            }
        }
        public ResultStatus GetAllDevices()
        {
            return SendRequest<DeviceResultList>("/Devices/getlist");
        }
        public ResultStatus GetDeviceById(Guid id)
        {
            return SendRequest<DeviceResult>("/Devices/getbyid?id=" + id);
        }
        public ResultStatus GetDeviceInformation(Guid id)
        {
            return SendRequest<DeviceResult>("/Devices/getdeviceinformation?id=" + id);
        }
        public ResultStatus GetDeviceActivationInformations()
        {
            var query = Helper.Json.Serialize(new
            {
                DataType = 0,
                Start = DateTime.Now.AddYears(-20),
                End = DateTime.Now
            });
            return SendRequest<DeviceResultList>("/Devices/GetDeviceActivationInformation", query);
        }
    }
}
