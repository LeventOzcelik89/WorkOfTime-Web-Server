using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_InvoiceItemReport 
    {
        public DateTime? created { get; set;}
        public Guid? invoiceId { get; set;}
        public Guid? productId { get; set;}
        public double? quantity { get; set;}
        public double? price { get; set;}
        public double? discount { get; set;}
        public short? discountType { get; set;}
        public int? itemOrder { get; set;}
        public string description { get; set;}
        public string Product_Title { get; set;}
        public short? stockType { get; set;}
        public double? stockCount { get; set;}
        public string Unit_Title { get; set;}
        public string Currency_Title { get; set;}
        public string Currency_Symbol { get; set;}
        public double? totalSubAmount { get; set;}
        public double? totalTax { get; set;}
        public double? totalAmount { get; set;}
        public double? discountTotal { get; set;}
    }
}
