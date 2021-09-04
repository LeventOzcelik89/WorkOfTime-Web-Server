using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectScopeLevelItems : InfolineTable
    {
        public Guid? projectId { get; set;}
        public Guid? scopeLevelId { get; set;}
        public Guid? locationId { get; set;}
    }
}
