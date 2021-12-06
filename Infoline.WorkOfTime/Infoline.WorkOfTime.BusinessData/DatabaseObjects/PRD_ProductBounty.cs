using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductBounty : InfolineTable
    {
        public double? amount { get; set;}
        public Guid? companyId { get; set;}
        public Guid? productId { get; set;}
        public int? year { get; set;}
        public int? month { get; set;}
    }
}
