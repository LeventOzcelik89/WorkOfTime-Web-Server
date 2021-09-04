using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonSchools : InfolineTable
    {
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public Guid? UserId { get; set;}
        public Guid? SchoolId { get; set;}
        public int? Level { get; set;}
        public string Branch { get; set;}
        public string area { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Level_Title { get; set;}
        public string User_Title { get; set;}
        public string School_Title { get; set;}
        public DateTime? JobEndDate { get; set;}
    }
}
