using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskFollowUpUser : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? userId { get; set;}
    }
}
