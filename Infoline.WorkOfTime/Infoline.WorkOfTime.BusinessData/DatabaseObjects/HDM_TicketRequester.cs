using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Yardım talebinde bulunan kişilerin tutulduğu tablodur.
    /// </summary>
    public partial class HDM_TicketRequester : InfolineTable
    {
        /// <summary>
        /// Talep eden kişinin ad soyad bilgisinin tutulduğu alandır.
        /// </summary>
        public string fullName { get; set;}
        /// <summary>
        /// Talep eden kişinin email adresinin tutulduğu alandır.
        /// </summary>
        public string email { get; set;}
        /// <summary>
        /// Talep eden kişinin telefon numarasının tutulduğu alandır.
        /// </summary>
        public string phone { get; set;}
        /// <summary>
        /// Talep eden kişinin işletmesinin tutulduğu alandır.
        /// </summary>
        public string company { get; set;}
    }
}
