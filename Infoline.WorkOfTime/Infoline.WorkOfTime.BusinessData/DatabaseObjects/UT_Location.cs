using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Lokasyonların tutulduğu tablodur.
    /// </summary>
    public partial class UT_Location : InfolineTable
    {
        /// <summary>
        /// Üst bölgenin id sinin tutulduğu alandır.
        /// </summary>
        public Guid? pid { get; set;}
        /// <summary>
        /// Lokasyon tipinin tutulduğu alandır.
        /// </summary>
        public int? type { get; set;}
        /// <summary>
        /// Lokasyon kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Lokasyon isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Lokasyon Alanının tutulduğu alandır.
        /// </summary>
        public IGeometry  polygon { get; set;}
    }
}
