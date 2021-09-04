using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PA_Advance : InfolineTable
    {
        public DateTime? progressDate { get; set;}
        public double? amount { get; set;}
        public Guid? currencyId { get; set;}
        public string description { get; set;}
        public short? type { get; set;}
        public short? direction { get; set;}
        public short? status { get; set;}
        public string rejectedDescription { get; set;}
        public DateTime? date { get; set;}
        public Guid? accountId { get; set;}
        public Guid? invoiceId { get; set;}
        public double? tax { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public DateTime? paymentRequestedDate { get; set;}
    }
}
