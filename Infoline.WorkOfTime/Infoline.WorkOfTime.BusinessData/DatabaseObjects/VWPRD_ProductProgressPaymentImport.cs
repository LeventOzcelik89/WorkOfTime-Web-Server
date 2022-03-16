using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductProgressPaymentImport : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? productId { get; set;}
        public DateTime? date { get; set;}
        public string imei { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string companyId_Title { get; set;}
    }
}
