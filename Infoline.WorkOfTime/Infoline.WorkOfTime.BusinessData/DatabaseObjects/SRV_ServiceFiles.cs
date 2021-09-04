using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Servis Dosyalarının bulunduğu tablodur . 
    /// </summary>
    public partial class SRV_ServiceFiles : InfolineTable
    {
        /// <summary>
        /// SRV_ServiceCalibration tablosundaki Id ile eşleşir. Servisin eşsiz kodudur. Servis kayıt bloğunun İlk kaydının id sidir.
        /// </summary>
        public Guid? ServiceId { get; set;}
        /// <summary>
        /// Olay Tipi 
        /// 
        /// ENUM => 0 = işe başlamadan once 1 = işe bittikten sonra 2 = Diğer
        /// </summary>
        public int? EventType { get; set;}
        /// <summary>
        /// Dosya İsmi
        /// </summary>
        public string FileName { get; set;}
    }
}
