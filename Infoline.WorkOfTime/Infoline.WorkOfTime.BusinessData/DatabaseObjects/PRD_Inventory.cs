using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Seri numaralı ürünlerin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_Inventory : InfolineTable
    {
        /// <summary>
        /// Envanter kodunun tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Envanter kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Envanter serial kodunun tutulduğu alandır.
        /// </summary>
        public string serialcode { get; set;}
        public short? type { get; set;}
    }
}
