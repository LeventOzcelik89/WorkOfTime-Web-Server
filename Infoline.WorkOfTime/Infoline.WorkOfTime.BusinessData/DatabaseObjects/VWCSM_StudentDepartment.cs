using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCSM_StudentDepartment : InfolineTable
    {
        public Guid? studentId { get; set;}
        public Guid? departmentId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string studentId_Title { get; set;}
        public string departmentId_Title { get; set;}
    }
}
