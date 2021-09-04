using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Dış bağlantıların tutulduğu tablodur.
    /// </summary>
    public partial class SYS_ExternalLinks : InfolineTable
    {
        /// <summary>
        /// Dış bağlantının tutulduğu alandır.
        /// </summary>
        public string Url { get; set;}
        /// <summary>
        /// Dış bağlantının açıklamasının alandır.
        /// </summary>
        public string Label { get; set;}
    }
}
