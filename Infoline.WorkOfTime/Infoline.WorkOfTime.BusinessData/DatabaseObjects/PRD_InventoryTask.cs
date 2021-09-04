using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Envanter görevlerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_InventoryTask : InfolineTable
    {
        /// <summary>
        /// Envanter görevinin başladığı tarihin tutulduğu alandır.
        /// </summary>
        public Guid? inventoryId { get; set;}
        /// <summary>
        /// Envanter görevinin başladığı tarihin tutulduğu alandır.
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Envanter görevinin biteceği tarihin tutulduğu alandır.
        /// </summary>
        public DateTime? endDate { get; set;}
        /// <summary>
        /// Görev periyodunun tutulduğu alandır.
        /// </summary>
        public double? period { get; set;}
        /// <summary>
        /// Envanter görev tipinin tutulduğu alandır.
        /// </summary>
        public int? type { get; set;}
        /// <summary>
        /// Şifrenin tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        public Guid? userId { get; set;}
        public Guid? companyId { get; set;}
    }
}
