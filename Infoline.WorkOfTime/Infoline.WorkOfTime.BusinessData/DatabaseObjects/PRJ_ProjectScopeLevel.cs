using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectScopeLevel : InfolineTable
    {
        public string level { get; set;}
        public Guid? projectId { get; set;}
    }
}
