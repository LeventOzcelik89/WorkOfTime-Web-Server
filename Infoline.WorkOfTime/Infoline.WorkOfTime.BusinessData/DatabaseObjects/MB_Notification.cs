using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Mobil uygulamalara gönderilen bildirim kayıtlarının tutulduğu tablodur.
    /// </summary>
    public partial class MB_Notification : InfolineTable
    {
        /// <summary>
        /// Bildirim Id
        /// </summary>
        public Guid IdNotification { get; set;}
        /// <summary>
        /// Bildirim Durumu
        /// </summary>
        public int Status { get; set;}
        /// <summary>
        /// Bildirim Tarihi
        /// </summary>
        public DateTime OperationDate { get; set;}
        /// <summary>
        /// Bildirim mesajı
        /// </summary>
        public string Message { get; set;}
        /// <summary>
        /// Bildirim kodu: Enum=> 0: EkipmanBakimBildirimi, 1:EkipmanGarantiBildirimi, 2:EkipmanKalibrasyonBildirimi, 3:IstasyonBakimBildirimi, 4:IstasyonGarantiBildirimi, 5:IstasyonKalibrasyonBildirimi, 6:EksikVeriBildirimi, 7:SupheliVeriBildirimi
        /// </summary>
        public int? EventCode { get; set;}
        /// <summary>
        /// Bildirim gönderilen kullanıcı Id
        /// </summary>
        public Guid? IdUserRef { get; set;}
    }
}
