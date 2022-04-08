using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductProgressPaymentImportSummary 
    {
        public Guid? DistributorId { get; set;}
        public Guid? ProductId { get; set;}
        public DateTime? date { get; set;}
        public int? AssignmentCount { get; set;}
    }
}
