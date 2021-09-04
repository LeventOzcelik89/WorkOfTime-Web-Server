using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_PerformanceMultiplier : InfolineTable
    {
        /// <summary>
        /// Kayıtların Gruplanabilmesi için rastgele id alanı
        /// </summary>
        public Guid? GroupId { get; set;}
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set;}
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set;}
        /// <summary>
        /// 0: Genel Performans Çarpanı, 1: Takım Lideri Çarpanı, 2: Satış Personeli Çarpanı
        /// </summary>
        public int? TargetGroupType { get; set;}
        /// <summary>
        /// Ürün / Ürün Grubu ID si. CRM_Product
        /// </summary>
        public Guid? ProductGroupId { get; set;}
        /// <summary>
        /// Minimum yüzdelik başlangıç değeri
        /// </summary>
        public int? MinPercentage { get; set;}
        public int? MaxPercentage { get; set;}
        /// <summary>
        /// Puan
        /// </summary>
        public int? Point { get; set;}
        /// <summary>
        /// Stratejik Focus için tanımlanan değer
        /// </summary>
        public bool? IsFocus { get; set;}
    }
}
