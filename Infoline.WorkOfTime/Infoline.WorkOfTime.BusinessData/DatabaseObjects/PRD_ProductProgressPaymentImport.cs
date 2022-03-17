using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductProgressPaymentImport : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? productId { get; set;}
        public DateTime? date { get; set;}
        public string imei { get; set;}
        public string companyTypes { get; set;}
    }
}
