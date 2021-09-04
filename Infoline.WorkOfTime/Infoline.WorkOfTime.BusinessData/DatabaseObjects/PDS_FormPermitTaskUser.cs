using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PDS_FormPermitTaskUser : InfolineTable
    {
        public Guid? formPermitTaskId { get; set;}
        public Guid? evaluatorId { get; set;}
        public Guid? evaluatedUserId { get; set;}
    }
}
