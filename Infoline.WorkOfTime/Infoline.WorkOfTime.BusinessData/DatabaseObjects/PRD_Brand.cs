using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün markalarının tutulduğu tablodur.
    /// </summary>
    public partial class PRD_Brand : InfolineTable
    {
        /// <summary>
        /// Marka isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
    }
}
