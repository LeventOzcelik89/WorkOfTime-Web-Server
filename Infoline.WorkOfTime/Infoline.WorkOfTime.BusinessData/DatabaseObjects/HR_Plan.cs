using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_Plan : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public DateTime? PlanDate { get; set;}
        public int? Result { get; set;}
        public string Description { get; set;}
        public Guid? PdsEvulateFormId { get; set;}
        public Guid? PdsEvulateFormResultId { get; set;}
        public bool? mailSend { get; set;}
        public Guid? CompanyId { get; set;}
        public Guid? StaffNeedsId { get; set;}
        public string contactLink { get; set;}
    }
}
