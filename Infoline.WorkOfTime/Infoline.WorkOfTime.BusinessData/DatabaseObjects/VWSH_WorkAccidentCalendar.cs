using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_WorkAccidentCalendar : InfolineTable
    {
        public Guid? workAccidentId { get; set;}
        public Guid? companyPersonCalendarId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string title { get; set;}
        public string description { get; set;}
        public int? type { get; set;}
        public string people_Title { get; set;}
    }
}
