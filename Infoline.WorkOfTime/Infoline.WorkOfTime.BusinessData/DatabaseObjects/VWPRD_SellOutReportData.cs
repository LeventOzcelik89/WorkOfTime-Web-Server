using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_SellOutReportData 
    {
        public Guid? DistributorId { get; set;}
        public string DistributorName { get; set;}
        public int? DistSalesCount { get; set;}
        public int? TemlikCount { get; set;}
    }
}
