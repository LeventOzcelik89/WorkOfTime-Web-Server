using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyDepartments : InfolineTable
    {
        public Guid? PID { get; set;}
        public string Name { get; set;}
        public Guid? ProjectId { get; set;}
        public int? Type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Project_Title { get; set;}
        public string PID_Title { get; set;}
        public string Type_Title { get; set;}
    }
}
