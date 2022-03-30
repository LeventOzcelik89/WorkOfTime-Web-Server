using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_BountyProduct : InfolineTable
    {
        public Guid? bountyId { get; set;}
        public Guid? productId { get; set;}
        public Guid? inventoryId { get; set;}
        public double? price { get; set;}
        public string imei { get; set;}
        public DateTime? salesDate { get; set;}
    }
}
