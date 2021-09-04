using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Servis işlemlerinin bulunduğu tablodur.
    /// </summary>
    public partial class SRV_Service : InfolineTable
    {
        /// <summary>
        /// SRV_Service tablosundaki id alanı ile eşleşir.
        /// 
        /// Recursive mantığı ile alt servisin devamındaki diğer servis işlemlerini tutmak için servis id si belirtilir.
        /// </summary>
        public Guid? PID { get; set;}
        /// <summary>
        /// Servis Durumu
        /// 
        /// ENUM
        /// 
        /// Talep Oluşturuldu
        /// 
        /// Doğrulama Kodu Girildi
        /// 
        /// Çözüm Bildirildi
        /// 
        /// Operatör Onayladı
        /// 
        /// Tekrar Kontrol
        /// </summary>
        public int? ServiceStatus { get; set;}
        /// <summary>
        /// Servis Durumu
        /// 
        /// ENUM
        /// 
        /// Arıza
        /// 
        /// Bakım
        /// 
        /// Kalibrasyon
        /// 
        /// Parça Değişikliği
        /// </summary>
        public int? ServiceType { get; set;}
        /// <summary>
        /// Servis Kodu
        /// </summary>
        public string ServiceCode { get; set;}
        /// <summary>
        /// INV_FixtureType tablosundaki id ile eşleşir  Servis Kategorisi malzeme eşsiz kodudur.
        /// </summary>
        public Guid? ServiceCategory { get; set;}
        /// <summary>
        /// INV_Fixture tablosundaki id ile eşleşir  Servis İle İlgili Demirbaş eşsiz kodudur
        /// </summary>
        public Guid? ServiceFixture { get; set;}
        /// <summary>
        /// STN_Station tablounda id ile eşleşir.İstasyon ID 
        /// </summary>
        public Guid? ServiceStationId { get; set;}
        /// <summary>
        /// INV_Company tablosundaki id ile eşleşir Servis işlemini yapacak Firmanın eşsiz kodunun girildiği alandır.
        /// </summary>
        public Guid? ServiceDeclarationFirm { get; set;}
        /// <summary>
        /// INV_Company_Person tablosundaki id ile eşleşir Servis işlemini yapacak Firma Personelinin eşsiz kodunun girildiği alandır.
        /// </summary>
        public Guid? ServiceDeclarationPerson { get; set;}
        /// <summary>
        /// Servis Detayı
        /// </summary>
        public string ServiceDetail { get; set;}
        /// <summary>
        /// Sistem tarafından yaratılan Doğrulama Kodu
        /// </summary>
        public string VerificationCode { get; set;}
        /// <summary>
        /// Doğrulama Kodu Girildi mi ?
        /// 
        /// 0=Pasif
        /// 
        /// 1=Aktif
        /// </summary>
        public bool? VerificationCodeConfirm { get; set;}
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Doğrulama Yapan Personel eşsiz kodudur
        /// </summary>
        public Guid? VerificationCodeConfirmPerson { get; set;}
        /// <summary>
        /// Doğrulama Yapılan Koordinat
        /// </summary>
        public IGeometry  VerificationCodeConfirmLocation { get; set;}
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Şervis operasyonunu yanıtlayan personelinin eşsiz kodudur.
        /// </summary>
        public Guid? ServiceOperationPerson { get; set;}
        /// <summary>
        /// Servis operasyonun yanıtlanma zamanı
        /// </summary>
        public DateTime? ServiceOperationDate { get; set;}
        /// <summary>
        /// Servis talebini yanıtlayan kişinin işlemle alakalı belirttiği detay
        /// </summary>
        public string ServiceOperationResultDetail { get; set;}
        /// <summary>
        /// Gerçekleşen servis işleminin gerçekleştiği yerin koordinat bilgisi
        /// </summary>
        public IGeometry  ServiceOperationLocation { get; set;}
        /// <summary>
        /// Gerçekleşen servis işleminin kontrol edildiği tarih
        /// </summary>
        public DateTime? ServiceOperationComfirmDate { get; set;}
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Şervis talep sonucunu onaylayan operatörün eşsiz kodudur.
        /// </summary>
        public Guid? ServiceOperationComfirmPerson { get; set;}
        /// <summary>
        /// Servis talep sonucu detayları
        /// </summary>
        public string ServiceOperationComfirmDetail { get; set;}
        /// <summary>
        /// Servis talebini onaylayan operatörün işlem ile ilgili cevabı. 
        /// 
        /// ENUM
        /// 
        /// Doğru şekilde gerçekleştirildi
        /// 
        /// Yeniden işlenmek için tekrarlandı
        /// 
        /// Onaylanmadı ve durduruldu
        /// </summary>
        public int? ServiceOperationComfirmResult { get; set;}
        /// <summary>
        /// Servis işleminin açıldığı andaki demirbaş veya istasyonun mevcut lokasyon bilgisi
        /// </summary>
        public IGeometry  CurrentLocation { get; set;}
        /// <summary>
        /// Servis işleminin açıldığı andaki demirbaş veya istasyonun mevcut lokasyon bilgisi
        /// 
        /// ENUM
        /// 
        /// depoda, istasyonda vb...
        /// </summary>
        public int? CurrentLocationStatus { get; set;}
        /// <summary>
        /// Bakımı yaklaşan vb gibi durumlarda 1 değerini alacak alan.
        /// </summary>
        public bool? AutoGenerated { get; set;}
    }
}
