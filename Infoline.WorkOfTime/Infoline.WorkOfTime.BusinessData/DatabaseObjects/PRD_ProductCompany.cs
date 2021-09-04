using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürünlerin tedarikçilerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_ProductCompany : InfolineTable
    {
        /// <summary>
        /// Ürün idsinin tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// İşletme idsinin tutulduğu alandır.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// İşletme ürün görev tipinin tutulduğu alandır.
        /// </summary>
        public int? type { get; set;}
    }
}
