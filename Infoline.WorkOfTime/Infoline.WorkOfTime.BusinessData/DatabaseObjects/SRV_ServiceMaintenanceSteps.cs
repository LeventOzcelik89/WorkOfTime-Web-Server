using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Servis Bakım Formunun Şablonlarının kaydeldiği tablodur . 
    /// </summary>
    public partial class SRV_ServiceMaintenanceSteps : InfolineTable
    {
        /// <summary>
        /// oluşturulan dinamik formun adı 
        /// </summary>
        public string FormText { get; set;}
        /// <summary>
        /// dinamik olarak yaratılan formun json veri karşılığında kaydedildiği alandır . 
        /// </summary>
        public string FormJson { get; set;}
    }
}
