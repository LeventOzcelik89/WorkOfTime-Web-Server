using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_ShiftTracking : InfolineTable
    {
        /// <summary>
        /// SH_User tablosundaki id.
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// CMP_Company tablosundaki id.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// 0- Mesaiye Başlandı, 1-Mesai Bitti, 2-Mola verildi, 3-Mola Bitti
        /// </summary>
        public short? shiftTrackingStatus { get; set;}
        /// <summary>
        /// Kullanıcının konumu
        /// </summary>
        public IGeometry  location { get; set;}
        /// <summary>
        /// Kullanıcının mesai durumunu bildirirken okuttuğu qr kod.
        /// </summary>
        public Guid? qrCodeData { get; set;}
        /// <summary>
        /// Kullanıcının mesai bildirimini yaptığı tarih.
        /// </summary>
        public DateTime? timestamp { get; set;}
        /// <summary>
        /// CMP_Storage, CMP_Company, PRD_Inventory tablolarından gelen id
        /// </summary>
        public Guid? tableId { get; set;}
        /// <summary>
        /// CMP_Storage, CMP_Company, PRD_Inventory tablo isimleri
        /// </summary>
        public string tableName { get; set;}
        public string qrCodeDataText { get; set;}
        /// <summary>
        /// PDKS cihazı ile kayıt gelmiş ise, Cihaz Id'si
        /// </summary>
        public Guid? shiftTrackingDeviceId { get; set;}
        /// <summary>
        /// Geçiş Tipini Gösterir (Pdks Parmak İzi, Pdks Şifre, Mobil Cihaz vs.)
        /// </summary>
        public int? passType { get; set;}
        /// <summary>
        /// Pdks cihazı ile kayıt oluşmuş ise cihazdaki user Id
        /// </summary>
        public string deviceUserId { get; set;}
    }
}
