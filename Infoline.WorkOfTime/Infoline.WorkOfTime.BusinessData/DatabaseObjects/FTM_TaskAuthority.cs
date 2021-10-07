using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskAuthority : InfolineTable
    {
        public Guid? projectId { get; set;}
        public Guid? customerId { get; set;}
        public Guid? userId { get; set;}
    }
}
