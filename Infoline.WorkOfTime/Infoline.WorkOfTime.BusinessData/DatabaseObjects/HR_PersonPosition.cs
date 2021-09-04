using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_PersonPosition : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public Guid? HrPositionId { get; set;}
    }
}
