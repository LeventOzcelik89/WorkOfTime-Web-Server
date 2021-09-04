using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectTypeLevel : InfolineTable
    {
        public string level { get; set;}
        public short? type { get; set;}
        public Guid? projectId { get; set;}
    }
}
