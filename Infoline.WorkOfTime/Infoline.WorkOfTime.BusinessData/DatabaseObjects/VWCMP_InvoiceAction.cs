using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_InvoiceAction : InfolineTable
    {
        public string description { get; set;}
        public Guid? invoiceId { get; set;}
        public short? type { get; set;}
        public Guid? transformInvoiceId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string createdBy_Photo { get; set;}
    }
}
