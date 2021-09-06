using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün materyellerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_ProductMateriel : InfolineTable
    {
        /// <summary>
        /// Ürün idsinin tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Materyel miktarının tutulduğu alandır.
        /// </summary>
        public double? quantity { get; set;}
        /// <summary>
        /// Materyel ürün idsinin tutulduğu alandır.
        /// </summary>
        public Guid? materialId { get; set;}
        public double? totalQuantity { get; set;}
    }
}
