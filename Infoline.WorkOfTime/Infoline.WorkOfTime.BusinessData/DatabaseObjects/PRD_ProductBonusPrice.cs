using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductBonusPrice : InfolineTable
    {
        public double? unitPrice { get; set;}
        public Guid? productBonusId { get; set;}
        public Guid? productId { get; set;}
    }
}
