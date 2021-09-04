using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üniversite bölümlerinin tutulduğu tablodur.
    /// </summary>
    public partial class CSM_Department : InfolineTable
    {
        /// <summary>
        /// Bölüm adının tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
    }
}
