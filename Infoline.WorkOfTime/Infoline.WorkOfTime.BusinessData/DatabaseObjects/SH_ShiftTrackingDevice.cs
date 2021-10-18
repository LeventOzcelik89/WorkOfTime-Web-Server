using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_ShiftTrackingDevice : InfolineTable
    {
        /// <summary>
        /// Cihaz İsmi
        /// </summary>
        public string DeviceName { get; set;}
        /// <summary>
        /// Cihaz Kodu
        /// </summary>
        public string DeviceCode { get; set;}
        /// <summary>
        /// Cihaz Markası
        /// </summary>
        public string DeviceBrand { get; set;}
        /// <summary>
        /// Cihaz Modeli
        /// </summary>
        public string DeviceModel { get; set;}
        /// <summary>
        /// Cihaz Seri No'su
        /// </summary>
        public string DeviceSerialNo { get; set;}
        /// <summary>
        /// Cihazın Konumu
        /// </summary>
        public IGeometry  Location { get; set;}
        /// <summary>
        /// Cihazın IP Addresi
        /// </summary>
        public string IPAddress { get; set;}
        /// <summary>
        /// Cihazın Alt Ağ Adresi
        /// </summary>
        public string SubnetAddress { get; set;}
        /// <summary>
        /// Cihazın Geçiş Adresi
        /// </summary>
        public string Gateway { get; set;}
        /// <summary>
        /// Cihazın Port Numarası
        /// </summary>
        public int? Port { get; set;}
        /// <summary>
        /// Cihazın Makine Numarası
        /// </summary>
        public int? MachineNumber { get; set;}
    }
}
