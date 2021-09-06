using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretim Operasyon Tablosu
    /// </summary>
    public partial class PRD_ProductionOperation : InfolineTable
    {
        /// <summary>
        /// Üretim
        /// </summary>
        public Guid? productionId { get; set;}
        /// <summary>
        /// Operasyon Durumu
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Operasyon Açıklaması
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Operasyona Eklenen Kullanıcı
        /// </summary>
        public Guid? userId { get; set;}
        public Guid? productionStageId { get; set;}
    }
}
