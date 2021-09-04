using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SYS_BlockMail : InfolineTable
    {
        public int? type { get; set;}
        public Guid? userId { get; set;}
    }
}
