using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_InvoiceItem : InfolineTable
    {
        public Guid? invoiceId { get; set;}
        public Guid? productId { get; set;}
        public double? quantity { get; set;}
        public Guid? unitId { get; set;}
        public double? price { get; set;}
        public double? KDV { get; set;}
        public short? KDVType { get; set;}
        public double? OIV { get; set;}
        public short? OIVType { get; set;}
        public double? OTV { get; set;}
        public short? OTVType { get; set;}
        public double? discount { get; set;}
        public short? discountType { get; set;}
        public string description { get; set;}
        public int? itemOrder { get; set;}
        public double? alternativeQuantity { get; set;}
        public Guid? alternativeUnitId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Product_Title { get; set;}
        public string Product_Image { get; set;}
        public string Unit_Title { get; set;}
        public string Currency_Title { get; set;}
        public string Currency_Symbol { get; set;}
        public short? stockType { get; set;}
        public short? invoiceType { get; set;}
        public short? invoiceStatus { get; set;}
        public string invoiceType_Title { get; set;}
        public string invoiceStatus_Title { get; set;}
        public double? totalSubAmount { get; set;}
        public double? totalTax { get; set;}
        public double? kdvAmount { get; set;}
        public double? totalAmount { get; set;}
        public string createdBy_Photo { get; set;}
    }
}
