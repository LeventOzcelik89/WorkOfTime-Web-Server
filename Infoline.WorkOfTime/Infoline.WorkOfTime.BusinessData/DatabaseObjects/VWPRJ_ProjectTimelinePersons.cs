using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectTimelinePersons : InfolineTable
    {
        public Guid? IdTimeline { get; set;}
        public Guid? IdProject { get; set;}
        public Guid? IdUser { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Project_Title { get; set;}
        public string IdUser_Title { get; set;}
        public string IdTimeline_Title { get; set;}
    }
}
