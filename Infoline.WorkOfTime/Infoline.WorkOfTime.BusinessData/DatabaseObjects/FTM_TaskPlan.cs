using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskPlan : InfolineTable
    {
        /// <summary>
        /// Görev Aktif / Pasif mi
        /// </summary>
        public bool? enabled { get; set;}
        public string name { get; set;}
        /// <summary>
        /// Görevin çalışacağı Başlangıç Tarihi
        /// </summary>
        public DateTime? frequencyStartDate { get; set;}
        /// <summary>
        /// Görevin çalışacağı Bitiş Tarihi
        /// </summary>
        public DateTime? frequencyEndDate { get; set;}
        /// <summary>
        /// Çalışma Sıklığı. Enum => Gün, Hafta, Ay, Yıl
        /// </summary>
        public int? frequency { get; set;}
        /// <summary>
        /// Gün Seçildiyse kaç günde bir, Hafta ise kaç haftada bir, Ay ise Kaç ayda bir vb seçimi için..
        /// </summary>
        public int? frequencyInterval { get; set;}
        /// <summary>
        /// Görev fiziki olarak ne zaman açılacak. Enum => Hemen, 1 Ay Önce, 1 Hafta Önce, 1 Gün Önce, Gününde
        /// </summary>
        public int? taskCreationTime { get; set;}
        /// <summary>
        /// Çalışacağı saat. (Gün seçildiyse çoklu)
        /// </summary>
        public string times { get; set;}
        /// <summary>
        /// Haftanın hangi günlerinde çalışacak.
        /// </summary>
        public string weekDays { get; set;}
        /// <summary>
        /// Ayın hangi günlerinde çalışacak.
        /// </summary>
        public string monthDays { get; set;}
        /// <summary>
        /// FTM_Task tablosu id alanı ile eşleşir.
        /// </summary>
        public Guid? templateId { get; set;}
        public int? monthFrequency { get; set;}
        public int? dayFrequency { get; set;}
    }
}
