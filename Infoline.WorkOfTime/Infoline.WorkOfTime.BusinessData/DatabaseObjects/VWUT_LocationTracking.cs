using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_LocationTracking : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? deviceId { get; set;}
        public string timeStamp { get; set;}
        public IGeometry  location { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string title { get; set;}
        public string device_Title { get; set;}
        public DateTime? takenDate { get; set;}
    }
}
