using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Konuların ve sorunların tutulduğu tablodur.
    /// </summary>
    public partial class HDM_Issue : InfolineTable
    {
        /// <summary>
        /// Üst konunun idsinin tutulduğu alandır.
        /// </summary>
        public Guid? pid { get; set;}
        /// <summary>
        /// Konunun taslak halinde mi yayında mı olduğunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Konu başlığının tutulduğu alandır.
        /// </summary>
        public string title { get; set;}
        /// <summary>
        /// Konunun çözülme süresinin tutulduğu alandır.
        /// </summary>
        public int? expiryMinute { get; set;}
        public Guid? mainId { get; set;}
    }
}
