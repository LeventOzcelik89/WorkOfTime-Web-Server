using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Mobil cihazlar üzerinden mobil uygulamalar aracılığıyla gerçekleşen tüm isteklerin kayıtlarının tutulduğu tablodur.
    /// </summary>
    public partial class MB_MobileDeviceRequests : InfolineTable
    {
        /// <summary>
        /// MB_MobileDevice Tablosundaki id alanı ile eşleşir.
        /// </summary>
        public Guid? DeviceId { get; set;}
        /// <summary>
        /// SH_Ticket tablosundaki id alanı ile eşleşir.
        /// </summary>
        public Guid? TicketId { get; set;}
        /// <summary>
        /// context.Request.Url.OriginalString
        /// </summary>
        public string Url { get; set;}
        /// <summary>
        /// context.Request.Browser.Browser
        /// </summary>
        public string Browser { get; set;}
        /// <summary>
        /// context.Request.TotalBytes
        /// </summary>
        public int? TotalBytes { get; set;}
        /// <summary>
        /// context.Request.Files
        /// </summary>
        public string PostedFiles { get; set;}
        /// <summary>
        /// context.Request.UserHostAddress
        /// </summary>
        public string IPAddress { get; set;}
    }
}
