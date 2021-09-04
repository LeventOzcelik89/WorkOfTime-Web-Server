using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aktivite ilişkilerinin tutulduğu tablodur.
    /// </summary>
    public partial class APM_ActivityUser : InfolineTable
    {
        /// <summary>
        /// İlişkili aktivite Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? activityId { get; set;}
        /// <summary>
        /// Katılımcı Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? userId { get; set;}
    }
}
