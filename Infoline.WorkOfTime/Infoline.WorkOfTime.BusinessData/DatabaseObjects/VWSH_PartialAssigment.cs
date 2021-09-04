using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PartialAssigment : InfolineTable
    {
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public double? courseHours { get; set;}
        public string schoolDepartment { get; set;}
        public string lesson { get; set;}
        public double? hourlyWage { get; set;}
        public string staffName { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
