using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_WorkAccidentCalendar : InfolineTable
    {
        public Guid? workAccidentId { get; set;}
        public Guid? companyPersonCalendarId { get; set;}
    }
}
