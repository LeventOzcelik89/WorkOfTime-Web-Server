using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductUnit : InfolineTable
    {
        public Guid? productId { get; set;}
        public Guid? unitId { get; set;}
        public double? quantity { get; set;}
        public short? isDefault { get; set;}
    }
}
