using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aktivitelerin tutulduğu tablodur.
    /// </summary>
    public partial class APM_Activity : InfolineTable
    {
        /// <summary>
        /// Aktivite isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Aktivite tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Aktivite başlangıç tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Aktivite bitiş tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? endDate { get; set;}
        /// <summary>
        /// Aktivite açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Aktivitenin ne tür bir aktivite olduğunun bilgisinin tutulduğu alandır
        /// </summary>
        public short? generalType { get; set;}
        /// <summary>
        /// Aktivitenin konumunun tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
        /// <summary>
        /// Aktivitenin iletişim bilgisinin tutulduğu alandır.
        /// </summary>
        public short? communicationType { get; set;}
        /// <summary>
        /// Aktivite bildiriminin tutulduğu alandır.
        /// </summary>
        public short? notification { get; set;}
    }
}
