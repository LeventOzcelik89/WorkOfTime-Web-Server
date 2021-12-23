using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_CustomerUser : InfolineTable
    {
        public Guid? name { get; set;}
        public Guid? serviceId { get; set;}
        public Guid? customerId { get; set;}
        public short? type { get; set;}
    }
}
