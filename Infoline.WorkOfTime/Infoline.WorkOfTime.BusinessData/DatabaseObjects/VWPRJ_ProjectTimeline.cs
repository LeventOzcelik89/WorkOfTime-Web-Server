using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectTimeline : InfolineTable
    {
        public Guid IdProject { get; set;}
        public string lastStatus { get; set;}
        public string actionPlan { get; set;}
        public int? Type { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string Name { get; set;}
        public string Description { get; set;}
        public short? Status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Type_Title { get; set;}
        public string Project_Title { get; set;}
        public string projectPersonIds { get; set;}
        public string Status_Title { get; set;}
        public string projectCode { get; set;}
    }
}
