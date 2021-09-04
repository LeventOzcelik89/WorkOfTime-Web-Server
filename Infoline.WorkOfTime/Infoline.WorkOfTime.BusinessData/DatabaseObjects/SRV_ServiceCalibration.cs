using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Servisin Kalibrasyon Değerlerinin bulunduğu tablo
    /// </summary>
    public partial class SRV_ServiceCalibration : InfolineTable
    {
        /// <summary>
        /// Mevcut Ölçülen Değer
        /// </summary>
        public double? CurrentMeasuredValue { get; set;}
        /// <summary>
        /// Gerçek Ölçülen Değer
        /// </summary>
        public double? RealMeasuredValue { get; set;}
        /// <summary>
        /// SRV_ServiceCalibration tablosundaki id ile eşleşir. Servisin eşsiz kodudur.
        /// </summary>
        public Guid? ServiceId { get; set;}
        /// <summary>
        /// İstasyon monitör Id
        /// </summary>
        public Guid? StationMonitorId { get; set;}
        /// <summary>
        /// Monitör Adı
        /// </summary>
        public string MonitorName { get; set;}
        /// <summary>
        /// Kalibrasyon detayının tutulduğu alandır.
        /// </summary>
        public string Details { get; set;}
        /// <summary>
        /// Kalibrasyon işlemi yapılan monitöre ait ölçü birimi
        /// </summary>
        public string Unit { get; set;}
    }
}
