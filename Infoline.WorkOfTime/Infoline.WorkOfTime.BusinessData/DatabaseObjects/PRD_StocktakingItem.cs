using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Stok sayım işlemi ürünlerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_StocktakingItem : InfolineTable
    {
        /// <summary>
        /// Sayım id sinin tutulduğu alandır.
        /// </summary>
        public Guid? stocktakingId { get; set;}
        /// <summary>
        /// Sayımı yapılan ürünün id sinin tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Sayımı yapılan envanterin seri numarasının tutulduğu alandır.
        /// </summary>
        public string serialNumber { get; set;}
        /// <summary>
        /// Sayım miktarı.
        /// </summary>
        public Single? quantity { get; set;}
        /// <summary>
        /// Sayım birimi.
        /// </summary>
        public Guid? unitId { get; set;}
    }
}
