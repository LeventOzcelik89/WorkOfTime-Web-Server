using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_LocationConfigUser : InfolineTable
    {
        public Guid? locationConfigId { get; set;}
        public Guid? userId { get; set;}
        public bool? isTrackingActive { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string configName { get; set;}
        public IGeometry  lastLocation { get; set;}
        public string timeStamp { get; set;}
        public string ProfilePhoto { get; set;}
        public string title { get; set;}
    }
}
