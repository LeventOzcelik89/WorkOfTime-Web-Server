using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskTemplateUserHelper : InfolineTable
    {
        public Guid? taskTemplateId { get; set;}
        public Guid? userId { get; set;}
    }
}
