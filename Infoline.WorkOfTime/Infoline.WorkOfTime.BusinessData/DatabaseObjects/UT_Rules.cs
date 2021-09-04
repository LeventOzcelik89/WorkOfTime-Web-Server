using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İzin, görevlendirme vs. süreci bulunan işlemlerin kurallarının tutulduğu tablodur.
    /// </summary>
    public partial class UT_Rules : InfolineTable
    {
        /// <summary>
        /// Kural adının tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Kural tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        public bool? isDefault { get; set;}
    }
}
