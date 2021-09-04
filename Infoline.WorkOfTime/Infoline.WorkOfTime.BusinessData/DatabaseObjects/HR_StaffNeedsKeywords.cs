using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_StaffNeedsKeywords : InfolineTable
    {
        public Guid? HrKeywordsId { get; set;}
        public Guid? HrStaffNeedsId { get; set;}
    }
}
