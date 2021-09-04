using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_GroupUsers : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? groupId { get; set;}
    }
}
