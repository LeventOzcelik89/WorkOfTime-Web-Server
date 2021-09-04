using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_PersonLanguages : InfolineTable
    {
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Kullanıcının eşsiz kodudur.
        /// </summary>
        public Guid? UserId { get; set;}
        /// <summary>
        /// Konuştuğu Dil
        /// </summary>
        public int? Languages { get; set;}
        /// <summary>
        /// Okuma Seviyesi
        /// </summary>
        public int? Reads { get; set;}
        /// <summary>
        /// Yazma Seviyesi
        /// </summary>
        public int? Write { get; set;}
        /// <summary>
        /// Konuşma Seviyesi
        /// </summary>
        public int? Speak { get; set;}
        public Guid? CertificateTypeId { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string CertificateName { get; set;}
        public int? CertificateTime { get; set;}
        public DateTime? ExpirationDate { get; set;}
        public double? point { get; set;}
    }
}
