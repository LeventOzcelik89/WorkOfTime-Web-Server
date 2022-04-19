using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_EntegrationActionSummary 
    {
        public Guid? DistributorId { get; set;}
        public string DistributorName { get; set;}
        public Guid? DealarId { get; set;}
        public Guid? ProductId { get; set;}
        public DateTime? date { get; set;}
        public string Imei { get; set;}
        public int? SalesCount { get; set;}
    }
}
