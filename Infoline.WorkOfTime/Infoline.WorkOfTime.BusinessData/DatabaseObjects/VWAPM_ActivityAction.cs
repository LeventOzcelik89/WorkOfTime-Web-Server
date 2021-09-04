using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWAPM_ActivityAction : InfolineTable
    {
        public Guid? activityId { get; set;}
        public short? type { get; set;}
        public string description { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string activityId_Title { get; set;}
        public string type_Title { get; set;}
    }
}
