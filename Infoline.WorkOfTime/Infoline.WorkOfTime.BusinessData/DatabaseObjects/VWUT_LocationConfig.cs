using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_LocationConfig : InfolineTable
    {
        public string configName { get; set;}
        public string shiftStart { get; set;}
        public string shiftEnd { get; set;}
        public int? dataSendingCount { get; set;}
        public string workDays { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string workDays_Title { get; set;}
    }
}
