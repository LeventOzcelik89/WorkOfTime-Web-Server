using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_UserEmailAccountShare : InfolineTable
    {
        public Guid? userIdFrom { get; set;}
        public Guid? userIdTo { get; set;}
        public bool? share { get; set;}
    }
}
