using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_ShiftTrackingDeviceUsers : InfolineTable
    {
        public Guid? deviceId { get; set;}
        public string deviceUserId { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string DeviceName { get; set;}
        public string DeviceSerialNo { get; set;}
    }
}
