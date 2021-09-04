using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_UserEmailAccount : InfolineTable
    {
        public string email { get; set;}
        public string password { get; set;}
        public int? mailType { get; set;}
        public int? isDefault { get; set;}
        public Guid? userid { get; set;}
    }
}
