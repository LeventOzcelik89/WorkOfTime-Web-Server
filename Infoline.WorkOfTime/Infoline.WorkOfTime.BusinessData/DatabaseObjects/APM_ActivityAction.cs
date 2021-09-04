using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aktivite ilişkilerinin tutulduğu tablodur.
    /// </summary>
    public partial class APM_ActivityAction : InfolineTable
    {
        /// <summary>
        /// İlişkili aktivite Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? activityId { get; set;}
        /// <summary>
        /// İşlem tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// İşlem açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
    }
}
