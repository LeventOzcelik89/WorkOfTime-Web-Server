using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_InventoryActionSummary 
    {
        public Guid? DistributorId { get; set;}
        public DateTime? date { get; set;}
        public Guid? ProductId { get; set;}
        public int? DistSalesCount { get; set;}
    }
}
