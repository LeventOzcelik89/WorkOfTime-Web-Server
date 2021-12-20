using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_Customer : InfolineTable
    {
        /// <summary>
        /// Müşteri Adı
        /// </summary>
        public Guid? name { get; set;}
        /// <summary>
        /// Müşteri Soyadı
        /// </summary>
        public Guid? lastName { get; set;}
        /// <summary>
        /// Müşteri Telefon Numarası
        /// </summary>
        public string phoneNumber { get; set;}
        /// <summary>
        /// Müşteri 2. telefon numarası
        /// </summary>
        public string otherPhoneNumber { get; set;}
        /// <summary>
        /// Açık Adres Alanı
        /// </summary>
        public Guid? openLocationId { get; set;}
        /// <summary>
        /// Açık Adres Detay
        /// </summary>
        public string Address { get; set;}
    }
}
