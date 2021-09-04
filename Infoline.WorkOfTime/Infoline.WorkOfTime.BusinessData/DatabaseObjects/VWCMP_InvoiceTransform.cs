using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_InvoiceTransform : InfolineTable
    {
        public string SerialNumberTo_Title { get; set;}
        public string SerialNumberFrom_Title { get; set;}
        public Guid? invoiceIdFrom { get; set;}
        public Guid? invoiceIdTo { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public short? typeFrom { get; set;}
        public string typeFrom_Title { get; set;}
        public short? typeTo { get; set;}
        public string typeTo_Title { get; set;}
        public short? direction { get; set;}
        public Guid? customerId { get; set;}
        public Guid? supplierId { get; set;}
        public string Supplier_Title { get; set;}
        public string Customer_Title { get; set;}
        public DateTime? issueDateTo { get; set;}
        public DateTime? issueDateFrom { get; set;}
        public short? statusTo { get; set;}
        public short? statusFrom { get; set;}
        public string serialNumberTo { get; set;}
        public string serialNumberFrom { get; set;}
        public string rowNumberTo { get; set;}
        public string rowNumberFrom { get; set;}
    }
}
