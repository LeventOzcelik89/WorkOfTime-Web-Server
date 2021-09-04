using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Önerilerin tutulduğu tablodur.
    /// </summary>
    public partial class HDM_Suggestion : InfolineTable
    {
        /// <summary>
        /// Öneri başlığının tutulduğu alandır.
        /// </summary>
        public string title { get; set;}
        /// <summary>
        /// Öneri içeriğinin tutulduğu alandır.
        /// </summary>
        public string content { get; set;}
        /// <summary>
        /// Önerinin yayında mı taslak mı olduğunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        public Guid? issueId { get; set;}
        public Guid? assignUserId { get; set;}
    }
}
