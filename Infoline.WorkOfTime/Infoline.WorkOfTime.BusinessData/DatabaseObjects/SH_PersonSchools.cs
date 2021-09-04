using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_PersonSchools : InfolineTable
    {
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public Guid? UserId { get; set;}
        public Guid? SchoolId { get; set;}
        public int? Level { get; set;}
        public string Branch { get; set;}
        public string area { get; set;}
    }
}
