using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_UserFireBaseToken : InfolineTable
    {
        public Guid? userId { get; set;}
        public string token { get; set;}
    }
}
