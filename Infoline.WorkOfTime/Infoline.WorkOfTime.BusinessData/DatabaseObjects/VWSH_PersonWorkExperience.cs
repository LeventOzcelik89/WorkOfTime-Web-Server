using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonWorkExperience : InfolineTable
    {
        public Guid? UserId { get; set;}
        public string CompanyName { get; set;}
        public DateTime? JobStartDate { get; set;}
        public DateTime? JobEndDate { get; set;}
        public string WorkingPosition { get; set;}
        public string JobDescription { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string User_Title { get; set;}
    }
}
