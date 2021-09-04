using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Banka tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class UT_Bank : InfolineTable
    {
        /// <summary>
        /// Banka adının tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Banka kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
    }
}
