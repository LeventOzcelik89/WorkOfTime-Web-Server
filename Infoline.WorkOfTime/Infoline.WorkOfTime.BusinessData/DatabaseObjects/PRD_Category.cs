using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün kategorilerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_Category : InfolineTable
    {
        /// <summary>
        /// Kategori isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        public Guid? pid { get; set;}
        public string code { get; set;}
    }
}
