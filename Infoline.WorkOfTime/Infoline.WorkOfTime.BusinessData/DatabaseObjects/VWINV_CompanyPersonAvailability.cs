using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPersonAvailability : InfolineTable
    {
        public Guid? IdUser { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public Guid? IdProject { get; set;}
        public double? rate { get; set;}
        public bool? isSalary { get; set;}
        public short? type { get; set;}
        public string Person_Title { get; set;}
        public string Project_Title { get; set;}
    }
}
