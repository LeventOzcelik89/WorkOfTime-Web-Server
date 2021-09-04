using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Yardım talebi üzerinde yapılan işlemlerin tutulduğu tablodur.
    /// </summary>
    public partial class HDM_TicketAction : InfolineTable
    {
        /// <summary>
        /// Talep eden kişinin ad soyad bilgisinin tutulduğu alandır.
        /// </summary>
        public Guid? ticketId { get; set;}
        /// <summary>
        /// İşlem tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Açıklama bilgisinin tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        public short? ticketStatus { get; set;}
    }
}
