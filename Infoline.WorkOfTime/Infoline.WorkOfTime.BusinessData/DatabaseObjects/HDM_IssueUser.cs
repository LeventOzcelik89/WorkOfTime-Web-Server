using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Konuyu çözebilecek kişilerin tutulduğu tablodur.
    /// </summary>
    public partial class HDM_IssueUser : InfolineTable
    {
        /// <summary>
        /// Konunun idsinin tutulduğu alandır.
        /// </summary>
        public Guid? issueId { get; set;}
        /// <summary>
        /// Konuyu çözebilecek kişinin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? userId { get; set;}
    }
}
