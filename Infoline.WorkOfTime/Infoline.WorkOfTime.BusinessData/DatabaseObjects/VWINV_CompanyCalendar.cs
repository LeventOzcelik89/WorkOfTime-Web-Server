using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyCalendar 
    {
        public Guid id { get; set;}
        public string Katilimcilar { get; set;}
        public string Description { get; set;}
        public int? Type { get; set;}
        public DateTime? created { get; set;}
        public Guid? createdby { get; set;}
        public DateTime? Start { get; set;}
        public DateTime? End { get; set;}
        public string Title { get; set;}
    }
}
