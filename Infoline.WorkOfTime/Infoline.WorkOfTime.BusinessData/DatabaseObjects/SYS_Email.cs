using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Sistem tarafından gönderilen email loglarının tutulduğu tablodur.
    /// </summary>
    public partial class SYS_Email : InfolineTable
    {
        /// <summary>
        /// Gönderilen mail adresleri
        /// </summary>
        public string SendingMail { get; set;}
        /// <summary>
        /// Mail başlığı
        /// </summary>
        public string SendingTitle { get; set;}
        /// <summary>
        /// Gönderilen mail içeriği
        /// </summary>
        public string SendingMessage { get; set;}
        /// <summary>
        /// Gönderilen mail html gövdesi
        /// </summary>
        public bool? SendingIsBodyHtml { get; set;}
        /// <summary>
        /// Mail gönderim durum mesajı
        /// </summary>
        public string Result { get; set;}
        /// <summary>
        /// Mail gönderim durumu
        /// </summary>
        public bool? Status { get; set;}
    }
}
