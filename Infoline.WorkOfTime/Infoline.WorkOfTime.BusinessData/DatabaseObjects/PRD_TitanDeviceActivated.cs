using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_TitanDeviceActivated : InfolineTable
    {
        /// <summary>
        /// Üretilen Cihazın Seri Numarası
        /// </summary>
        public string SerialNumber { get; set;}
        /// <summary>
        /// Üretilen Ürürünün IMEI1 Kodu
        /// </summary>
        public string IMEI1 { get; set;}
        /// <summary>
        /// Üretilen Ürürünün IMEI2 Kodu
        /// </summary>
        public string IMEI2 { get; set;}
        /// <summary>
        /// Cihazın açılma tarihi
        /// </summary>
        public DateTime? CreatedOfTitan { get; set;}
        /// <summary>
        /// Cihaz Numarası
        /// </summary>
        public Guid? DeviceId { get; set;}
        /// <summary>
        /// Üretilen Ürün
        /// </summary>
        public Guid? ProductId { get; set;}
        /// <summary>
        /// Envanter 
        /// </summary>
        public Guid? InventoryId { get; set;}
    }
}
