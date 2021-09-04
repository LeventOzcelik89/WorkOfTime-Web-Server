using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aktivite ilişkilerinin tutulduğu tablodur.
    /// </summary>
    public partial class APM_ActivityRelation : InfolineTable
    {
        /// <summary>
        /// İlişkili aktivite Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? activityId { get; set;}
        /// <summary>
        /// İlişkili kaydın Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? dataId { get; set;}
        /// <summary>
        /// İlişkili tablo isminin tutulduğu alandır.
        /// </summary>
        public string dataTable { get; set;}
    }
}
