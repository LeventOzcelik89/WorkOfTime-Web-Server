using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Sektör tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class UT_Sector : InfolineTable
    {
        /// <summary>
        /// Üst sektör id sinin tutulduğu alandır.
        /// </summary>
        public Guid? pid { get; set;}
        /// <summary>
        /// Sektör isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
    }
}
