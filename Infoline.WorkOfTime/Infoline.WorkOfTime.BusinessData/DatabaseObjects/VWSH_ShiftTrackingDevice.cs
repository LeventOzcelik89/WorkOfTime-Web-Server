using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_ShiftTrackingDevice : InfolineTable
    {
        public string DeviceName { get; set;}
        public string DeviceCode { get; set;}
        public string DeviceBrand { get; set;}
        public string DeviceModel { get; set;}
        public string DeviceSerialNo { get; set;}
        public IGeometry  Location { get; set;}
        public string IPAddress { get; set;}
        public string SubnetAddress { get; set;}
        public string Gateway { get; set;}
        public int? Port { get; set;}
        public int? MachineNumber { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
