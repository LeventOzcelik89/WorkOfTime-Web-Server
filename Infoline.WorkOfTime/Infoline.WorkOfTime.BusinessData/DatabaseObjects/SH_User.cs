using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Kullanıcı bilgilerinin bulunduğu tablodur.
    /// </summary>
    public partial class SH_User : InfolineTable
    {
        /// <summary>
        /// Durum
        /// </summary>
        public bool? status { get; set;}
        /// <summary>
        /// Tip
        /// </summary>
        public int? type { get; set;}
        /// <summary>
        /// Kod
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string loginname { get; set;}
        /// <summary>
        /// Kişi Adı
        /// </summary>
        public string firstname { get; set;}
        /// <summary>
        /// Kişi Soyadı
        /// </summary>
        public string lastname { get; set;}
        /// <summary>
        /// Doğum Tarihi
        /// </summary>
        public DateTime? birthday { get; set;}
        /// <summary>
        /// Şifre
        /// </summary>
        public string password { get; set;}
        /// <summary>
        /// Ünvan
        /// </summary>
        public string title { get; set;}
        /// <summary>
        /// E posta
        /// </summary>
        public string email { get; set;}
        /// <summary>
        /// Telefon
        /// </summary>
        public string phone { get; set;}
        /// <summary>
        /// Cep Telefonu
        /// </summary>
        public string cellphone { get; set;}
        /// <summary>
        /// Adres
        /// </summary>
        public string address { get; set;}
        /// <summary>
        /// Kullanıcının yaşadğı lokasyon
        /// </summary>
        public Guid? locationId { get; set;}
        /// <summary>
        /// Şirket Cep Telefonu
        /// </summary>
        public string companyCellPhone { get; set;}
        /// <summary>
        /// Şirket Cep Telefonu Kısa Kod
        /// </summary>
        public string companyCellPhoneCode { get; set;}
        /// <summary>
        /// Şirket Sabit Telefeonu
        /// </summary>
        public string companyOfficePhone { get; set;}
        /// <summary>
        /// Şirket Sabit Telefon Dahili Numarası
        /// </summary>
        public string companyOfficePhoneCode { get; set;}
    }
}
