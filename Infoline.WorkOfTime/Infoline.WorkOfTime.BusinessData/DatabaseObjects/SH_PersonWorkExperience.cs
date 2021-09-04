using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_PersonWorkExperience : InfolineTable
    {
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Kullanıcının eşsiz kodudur.
        /// </summary>
        public Guid? UserId { get; set;}
        /// <summary>
        /// Çalıştığı Firmanın Adı
        /// </summary>
        public string CompanyName { get; set;}
        /// <summary>
        /// İşe Girişi Tarihi
        /// </summary>
        public DateTime? JobStartDate { get; set;}
        /// <summary>
        /// İşten Çıkış Tarihi
        /// </summary>
        public DateTime? JobEndDate { get; set;}
        /// <summary>
        /// Çalıştığı Firmadaki Pozisyonu (Örn: Yazılım Uzmanı)
        /// </summary>
        public string WorkingPosition { get; set;}
        /// <summary>
        /// Çalıştığı Firmanın İş Tanımı
        /// </summary>
        public string JobDescription { get; set;}
    }
}
