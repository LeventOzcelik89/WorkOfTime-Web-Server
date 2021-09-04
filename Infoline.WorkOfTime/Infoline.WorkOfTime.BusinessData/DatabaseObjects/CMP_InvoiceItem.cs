using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_InvoiceItem : InfolineTable
    {
        /// <summary>
        /// Kalemin hangi fatura üzerinde olduğunu gösteren alandır.
        /// </summary>
        public Guid? invoiceId { get; set;}
        /// <summary>
        /// Ürünün tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Ürünün kaç adet olduğunu belirten alandır.
        /// </summary>
        public double? quantity { get; set;}
        /// <summary>
        /// Ürünün biriminin tutulduğu alandır.
        /// </summary>
        public Guid? unitId { get; set;}
        /// <summary>
        /// Üünün birim fiyatıının tutulduğu alandır.
        /// </summary>
        public double? price { get; set;}
        /// <summary>
        /// Ürünün KDV'sinin tutulduğu alandır.
        /// </summary>
        public double? KDV { get; set;}
        /// <summary>
        /// KDV'nin tipinin tutulduğu alandır. (Yüzde)
        /// </summary>
        public short? KDVType { get; set;}
        /// <summary>
        /// OIV'nin tutulduğu alandır.
        /// </summary>
        public double? OIV { get; set;}
        public short? OIVType { get; set;}
        public double? OTV { get; set;}
        public short? OTVType { get; set;}
        /// <summary>
        /// Ürüne yapılan indirimin tutulduğu alandır.
        /// </summary>
        public double? discount { get; set;}
        /// <summary>
        /// Ürüne yapılan indirimin tipinin tutulduğu alandır. (Yüzde - Tutar)
        /// </summary>
        public short? discountType { get; set;}
        /// <summary>
        /// Ürünün açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Ürünün gösterimde sıralamasını tutan alandır.
        /// </summary>
        public int? itemOrder { get; set;}
    }
}
