using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyDepartments : InfolineTable
    {
        public Guid? PID { get; set;}
        public string Name { get; set;}
        public Guid? ProjectId { get; set;}
        public int? Type { get; set;}
    }
}
