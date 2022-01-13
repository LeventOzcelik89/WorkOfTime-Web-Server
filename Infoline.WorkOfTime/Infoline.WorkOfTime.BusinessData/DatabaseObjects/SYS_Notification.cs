using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SYS_Notification : InfolineTable
    {
        /// <summary>
        /// Bildirim Gönderilen Kullanıcı
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// Bildirim İçeriği(body)
        /// </summary>
        public string message { get; set;}
        /// <summary>
        /// Mesaj Başlığı (Title)
        /// </summary>
        public string title { get; set;}
        /// <summary>
        /// KEY_1
        /// </summary>
        public string url { get; set;}
        /// <summary>
        /// KEY_2
        /// </summary>
        public string paramaters { get; set;}
    }
}
