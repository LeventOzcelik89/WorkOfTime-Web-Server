using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Yardım talebi üzerinde yapılan mesaj işlemlerinin tutulduğu tablodur.
    /// </summary>
    public partial class HDM_TicketMessage : InfolineTable
    {
        /// <summary>
        /// Talebin tutulduğu alandır.
        /// </summary>
        public Guid? ticketId { get; set;}
        /// <summary>
        /// Mesaj tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Mesaj içeriğinin tutulduğu alandır.
        /// </summary>
        public string content { get; set;}
        /// <summary>
        /// Mesaj gönderilirken cc ye eklenecek mail adresilerinin tutulduğu alandır.
        /// </summary>
        public string cc { get; set;}
        /// <summary>
        /// Mesaj gönderilirken bcc ye eklenecek mail adresilerinin tutulduğu alandır.
        /// </summary>
        public string bcc { get; set;}
        public string mailto { get; set;}
    }
}
