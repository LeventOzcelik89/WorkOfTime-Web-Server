using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductBounty : InfolineTable
    {
        public double? price { get; set;}
        public Guid? companyId { get; set;}
        public Guid? personId { get; set;}
        public Guid? productId { get; set;}
    }
}
