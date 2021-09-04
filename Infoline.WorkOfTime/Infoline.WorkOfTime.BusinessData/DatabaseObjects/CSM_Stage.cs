using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aktivite aşamalarının tutulduğu tablodur.
    /// </summary>
    public partial class CSM_Stage : InfolineTable
    {
        /// <summary>
        /// Aşama kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Aşama isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Aşamanın durumunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Aşama açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Aşama renginin tutulduğu alandır.
        /// </summary>
        public string color { get; set;}
    }
}
