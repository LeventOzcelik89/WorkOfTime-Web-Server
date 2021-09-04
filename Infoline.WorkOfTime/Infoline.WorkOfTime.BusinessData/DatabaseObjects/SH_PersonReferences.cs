using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_PersonReferences : InfolineTable
    {
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Kullanıcının eşsiz kodudur.
        /// </summary>
        public Guid? UserId { get; set;}
        /// <summary>
        /// Referans verilen kişinin adı soyadı
        /// </summary>
        public string ReferenceUserName { get; set;}
        /// <summary>
        /// Referans verilen kişinin şirketteki pozisyonu
        /// </summary>
        public string ReferencePosition { get; set;}
        /// <summary>
        /// Referans verilen kişinin mail adresi
        /// </summary>
        public string ReferenceMail { get; set;}
        /// <summary>
        /// Referans verilen kişinin telefon numarası
        /// </summary>
        public string ReferencePhone { get; set;}
        /// <summary>
        /// Referans verilen kişinin çalıştığı firmanın adı
        /// </summary>
        public string ReferenceWorkingCompany { get; set;}
    }
}
