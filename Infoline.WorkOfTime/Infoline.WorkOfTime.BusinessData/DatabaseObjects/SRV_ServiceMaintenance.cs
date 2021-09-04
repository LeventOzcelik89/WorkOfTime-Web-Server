using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Servis Bakım tablosudur.
    /// </summary>
    public partial class SRV_ServiceMaintenance : InfolineTable
    {
        /// <summary>
        /// SRV_ServiceMaintenance tablosundaki id ile eşleşir. Bakımın  eşsiz kodudur.
        /// </summary>
        public Guid? maintenanceStepId { get; set;}
        /// <summary>
        /// SRV_ServiceMaintenance tablosundaki id ile eşleşir. Servisin eşsiz kodudur.
        /// </summary>
        public Guid? serviceId { get; set;}
        /// <summary>
        /// SRV_ServiceMaintenance tablosundaki id alanı ile eşleşir.
        /// 
        /// Recursive mantığı ile servis  id si belirtilir.
        /// </summary>
        public Guid? servicePID { get; set;}
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
