using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FVR_Favorites : InfolineTable
    {
        public Guid? userId { get; set;}
        public string Area { get; set;}
        public string Controller { get; set;}
        public string Method { get; set;}
        public int? Count { get; set;}
        public bool? Status { get; set;}
        public string Action { get; set;}
        public string Description { get; set;}
    }
}
