using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Servis Donanım Değişikliklerinin tablosu 
    /// </summary>
    public partial class SRV_ServiceHardwareChange : InfolineTable
    {
        /// <summary>
        /// SRV_ServiceHardwareChange tablosundaki id ile eşleşir. Servisin eşsiz kodudur.
        /// </summary>
        public Guid? serviceId { get; set;}
        /// <summary>
        /// 
        /// 
        /// SRV_ServiceHardwareChange tablosundaki id alanı ile eşleşir.
        /// 
        /// Recursive mantığı ile servis id si belirtilir.
        /// </summary>
        public Guid? servicePID { get; set;}
        /// <summary>
        /// Değişim Tipi
        /// 
        /// Enum --> Takıldı , Söküldü
        /// </summary>
        public int? changeType { get; set;}
        /// <summary>
        /// Açıklama
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// INV_Fixture tablosundaki id ile eşleşir. Servisin eşsiz kodudur.
        /// </summary>
        public Guid? fixtureId { get; set;}
        /// <summary>
        /// Dosya Yolu
        /// </summary>
        public string FilePath { get; set;}
    }
}
