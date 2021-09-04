using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWAPM_ActivityOthers 
    {
        public int type { get; set;}
        public string description { get; set;}
        public string activityUserIds { get; set;}
        public DateTime? created { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string name { get; set;}
    }
}
