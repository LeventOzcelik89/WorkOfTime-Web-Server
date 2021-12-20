using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Lokasyon için gerekli konfigürasyonların tutulduğu tablo.
    /// </summary>
    public partial class UT_LocationConfig : InfolineTable
    {
        public string configName { get; set;}
        /// <summary>
        /// Konum toplanacak başlangıç saat bilgisi
        /// </summary>
        public string shiftStart { get; set;}
        /// <summary>
        /// Konum toplanacak bitiş saat bilgisi
        /// </summary>
        public string shiftEnd { get; set;}
        /// <summary>
        /// Toplanan konumlar kaçar kaçar merkeze gönderilecek.
        /// </summary>
        public int? dataSendingCount { get; set;}
        /// <summary>
        /// Konumun toplanacağı günler. her bir karakter günü temsil eder. Pazartesi, Salı, Çarşamba,... Pazar
        /// </summary>
        public string workDays { get; set;}
        /// <summary>
        /// Mesai Başlangıç Tarihinin Ve Saatinin Tutulduğu Tablo
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Mesai Bitiş Tarihinin Ve Saatinin Tutulduğu Tablo
        /// </summary>
        public DateTime? endDate { get; set;}
    }
}
