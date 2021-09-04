using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class PRD_Product : InfolineTable
    {
        /// <summary>
        /// Ürün kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Ürün barkodunun tutulduğu alandır.
        /// </summary>
        public string barcode { get; set;}
        /// <summary>
        /// Ürün isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Ürün açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Ürün kategori idsinin tutulduğu alandır.
        /// </summary>
        public Guid? categoryId { get; set;}
        /// <summary>
        /// Ürün marka idsinin tutulduğu alandır.
        /// </summary>
        public Guid? brandId { get; set;}
        /// <summary>
        /// Ürün birim idsinin tutulduğu alandır.
        /// </summary>
        public Guid? unitId { get; set;}
        /// <summary>
        /// Ürün tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Ürün durumunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Ürün barkod tipinin tutulduğu alandır.
        /// </summary>
        public short? barcodeType { get; set;}
        public bool? isCriticalStock { get; set;}
        public int? criticalStock { get; set;}
        /// <summary>
        /// Stok takip tipinin tutulduğu alandır.
        /// </summary>
        public short? stockType { get; set;}
    }
}
