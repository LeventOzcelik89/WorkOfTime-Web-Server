using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_PermitOffical : InfolineTable
    {
        public int? Type { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Type_Title { get; set;}
    }
}
