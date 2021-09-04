using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_PlanPerson : InfolineTable
    {
        public Guid? HrPlanId { get; set;}
        public Guid? UserId { get; set;}
    }
}
