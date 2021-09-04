using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class UT_LocationConfigUser : InfolineTable
    {
        public Guid? locationConfigId { get; set;}
        public Guid? userId { get; set;}
        public bool? isTrackingActive { get; set;}
    }
}
