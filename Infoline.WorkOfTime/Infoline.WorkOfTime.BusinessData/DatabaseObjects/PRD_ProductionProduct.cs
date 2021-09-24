using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretimde Olan Ürünler Tablosu
    /// </summary>
    public partial class PRD_ProductionProduct : InfolineTable
    {
        /// <summary>
        /// Üretim Kaydı
        /// </summary>
        public Guid? productionId { get; set;}
        /// <summary>
        /// Ürün Kaydı
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Ürün Seri Kodları
        /// </summary>
        public string serialCodes { get; set;}
        /// <summary>
        /// Ürün Miktarı
        /// </summary>
        public double? quantity { get; set;}
        /// <summary>
        /// Toplam Gerekli Miktar
        /// </summary>
        public double? totalQuantity { get; set;}
        /// <summary>
        /// Harcanan Miktar
        /// </summary>
        public double? amountSpent { get; set;}
        /// <summary>
        /// Fiyat
        /// </summary>
        public double? price { get; set;}
        /// <summary>
        /// Ürün Ağacı Kaydı
        /// </summary>
        public Guid? materialId { get; set;}
        /// <summary>
        /// Tip
        /// </summary>
        public short? type { get; set;}
        public short? transactionType { get; set;}
        public Guid? currencyId { get; set;}
        public Guid? unitId { get; set;}
    }
}
