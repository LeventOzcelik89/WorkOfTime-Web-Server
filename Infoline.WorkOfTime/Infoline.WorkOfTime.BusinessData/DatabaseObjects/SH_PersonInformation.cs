using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_PersonInformation : InfolineTable
    {
        /// <summary>
        /// SH_User id.si
        /// </summary>
        public Guid? UserId { get; set;}
        /// <summary>
        /// Kişisel Bilgiler Uyruk. T.C + Diğer olacak radio buttonlarda. Diğer seçilirse textbox u dolduracak
        /// </summary>
        public string Nationality { get; set;}
        /// <summary>
        /// KB Cinsiyet : Enum Erkek, Kadın
        /// </summary>
        public int? Gender { get; set;}
        /// <summary>
        /// KB Medeni Durum : Enum Evli, Bekar
        /// </summary>
        public int? MaritalStatus { get; set;}
        /// <summary>
        /// KB Askerlik Durumu. ENUM > Yapıldı. Yapılmadı. Muaf. Yabancı. Tecilli
        /// </summary>
        public int? Military { get; set;}
        /// <summary>
        /// KB Askerlik Terhis Tarihi
        /// </summary>
        public DateTime? MilitaryDoneDate { get; set;}
        /// <summary>
        /// KB Askerlik Süresi ( Ay Cinsinden ) 
        /// </summary>
        public int? MilitaryDoneDuration { get; set;}
        /// <summary>
        /// KB Askerlik Muaf Açıklaması
        /// </summary>
        public string MilitaryExemptDetail { get; set;}
        /// <summary>
        /// KB Askerlik Tecil Sebebi
        /// </summary>
        public string MilitaryProbationDetail { get; set;}
        /// <summary>
        /// KB Askerlik Tecilli ise Tecil olduğu tarih
        /// </summary>
        public DateTime? MilitaryProbationDate { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfus Cüzdanı Seri No -- Y13 24023 Maskesi ile beraber
        /// </summary>
        public string IDSerialNumber { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfus Cüzdanı Anne Adı
        /// </summary>
        public string IDMotherName { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : NC Baba Adı
        /// </summary>
        public string IDFatherName { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : NC Doğum Yeri (Şehir) City Tablosu
        /// </summary>
        public Guid? IDBornLocation { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : NC Kan Grubu. ENUM Olacak.
        /// </summary>
        public int? IDBloodGroup { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Önceki Soyismi
        /// </summary>
        public string IDPreviousSurname { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfusa Kayıtlı Olduğu İl
        /// </summary>
        public Guid? IDCity { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfusa Kayıtlı Olduğu İlçe
        /// </summary>
        public Guid? IDTown { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfusa Kayıtlı Olduğu Mahalle
        /// </summary>
        public string IDDistrict { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfusa Kayıtlı Olduğu  Köy
        /// </summary>
        public string IDVillage { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Cüzdan Verildiği Yer
        /// </summary>
        public string IDDeliveryLocation { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Cüzdan Veriliş Sebebi
        /// </summary>
        public string IDDeliveryDetail { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Cüzdan Kayıt No
        /// </summary>
        public string IDRecordNumber { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Cüzdan Veriliş Tarihi
        /// </summary>
        public DateTime? IDDeliveryDate { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfüsa Kayıtlı Olduğu Cilt No
        /// </summary>
        public string IDVolumeNumber { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfusa Kayıtlı Olduğu Aile Sıra No
        /// </summary>
        public string IDFamilyNumber { get; set;}
        /// <summary>
        /// Kimlik Bilgileri : Nüfusa Kayıtlı Olduğu Sıra Numarası
        /// </summary>
        public string IDRowNumber { get; set;}
        /// <summary>
        /// Kişisel mail
        /// </summary>
        public string PersonalMail { get; set;}
        /// <summary>
        /// Ev Telefonu 
        /// </summary>
        public string PersonalHomePhone { get; set;}
        /// <summary>
        /// Acil durumda bağlantı kurulacak Kişi Adı soyadı
        /// </summary>
        public string EmergencyName { get; set;}
        /// <summary>
        /// Acil durumda bağlantı kurulacak Kişi Telefonu
        /// </summary>
        public string EmergencyPhone { get; set;}
        /// <summary>
        /// Acil durumda bağlantı kurulacak Kişi Maili
        /// </summary>
        public string EmergencyMail { get; set;}
        /// <summary>
        /// Acil durumda bağlantı kurulacak Kişi Yakınlığı
        /// </summary>
        public string EmergencyProximity { get; set;}
        /// <summary>
        /// Dini
        /// </summary>
        public string Religious { get; set;}
        /// <summary>
        /// Doğum Yeri : İlçe
        /// </summary>
        public string IDBornTownLocation { get; set;}
        /// <summary>
        /// Sigorta Sicil Numarası
        /// </summary>
        public string InsuranceIdentityNumber { get; set;}
        /// <summary>
        /// Kimlik Numarası
        /// </summary>
        public string IdentificationNumber { get; set;}
        public bool? hasAgi { get; set;}
    }
}
