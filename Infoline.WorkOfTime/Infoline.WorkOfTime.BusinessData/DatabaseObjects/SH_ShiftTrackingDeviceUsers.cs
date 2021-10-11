using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_ShiftTrackingDeviceUsers : InfolineTable
    {
        /// <summary>
        /// SH_ShiftTrackingDevice Id
        /// </summary>
        public Guid? deviceId { get; set;}
        /// <summary>
        /// PDKS cihazındaki user Id
        /// </summary>
        public string deviceUserId { get; set;}
        /// <summary>
        /// SH_User daki User Id
        /// </summary>
        public Guid? userId { get; set;}
    }
}
