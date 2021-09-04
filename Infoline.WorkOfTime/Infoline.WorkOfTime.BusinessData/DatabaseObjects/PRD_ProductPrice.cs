using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün Fiyatlarının Tutulduğu Tablodur.
    /// </summary>
    public partial class PRD_ProductPrice : InfolineTable
    {
        /// <summary>
        /// Ürün bilgisinin tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Tutar bilgisinin tutulduğu alandır.
        /// </summary>
        public double? price { get; set;}
        /// <summary>
        /// Fiyat tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Para biriminin tutulduğu alandır.
        /// </summary>
        public Guid? currencyId { get; set;}
    }
}
