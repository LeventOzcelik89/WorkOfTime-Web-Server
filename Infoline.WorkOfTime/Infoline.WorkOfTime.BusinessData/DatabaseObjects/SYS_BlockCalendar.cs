using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SYS_BlockCalendar : InfolineTable
    {
        public int? type { get; set;}
        public Guid? userId { get; set;}
    }
}
