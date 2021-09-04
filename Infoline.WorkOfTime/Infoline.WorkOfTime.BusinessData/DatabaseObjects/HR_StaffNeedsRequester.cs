using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_StaffNeedsRequester : InfolineTable
    {
        public Guid? StaffNeedId { get; set;}
        public Guid? RequestId { get; set;}
    }
}
