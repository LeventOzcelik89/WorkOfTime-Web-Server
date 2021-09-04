using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Öğrencilerin okullarının tutulduğu tablodur.
    /// </summary>
    public partial class CSM_School : InfolineTable
    {
        /// <summary>
        /// Okul adının tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Okulun konum bilgisinin tutulduğu alandır.
        /// </summary>
        public Guid? locationId { get; set;}
    }
}
