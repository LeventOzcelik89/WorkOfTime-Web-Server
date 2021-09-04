using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün görevlerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_ProductTask : InfolineTable
    {
        /// <summary>
        /// Ürün görevinin başladığı tarihi tutan alandır.
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Ürün görevinin biteceği tarihi tutan alandır.
        /// </summary>
        public DateTime? endDate { get; set;}
        /// <summary>
        /// Ürün görev periyodunun tutulduğu alandır.
        /// </summary>
        public double? period { get; set;}
        /// <summary>
        /// Ürün görev tipinin tutulduğu alandır.
        /// </summary>
        public int? type { get; set;}
        /// <summary>
        /// Ürün görev açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        public Guid? productId { get; set;}
    }
}
