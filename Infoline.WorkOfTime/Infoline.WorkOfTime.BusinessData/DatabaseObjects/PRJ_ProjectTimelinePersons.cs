using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectTimelinePersons : InfolineTable
    {
        public Guid? IdTimeline { get; set;}
        public Guid? IdProject { get; set;}
        public Guid? IdUser { get; set;}
    }
}
