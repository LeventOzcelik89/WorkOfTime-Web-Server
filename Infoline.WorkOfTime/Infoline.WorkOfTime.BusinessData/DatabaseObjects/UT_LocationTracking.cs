using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class UT_LocationTracking : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? deviceId { get; set;}
        public string timeStamp { get; set;}
        public IGeometry  location { get; set;}
    }
}
