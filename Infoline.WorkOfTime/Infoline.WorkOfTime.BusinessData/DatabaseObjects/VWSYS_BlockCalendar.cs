using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSYS_BlockCalendar : InfolineTable
    {
        public int? type { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string email { get; set;}
    }
}
