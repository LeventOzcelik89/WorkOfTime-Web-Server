using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretim Operasyon Tablosu
    /// </summary>
    public partial class PRD_ProductionUser : InfolineTable
    {
        /// <summary>
        /// Üretim Kaydı
        /// </summary>
        public Guid? productionId { get; set;}
        /// <summary>
        /// Atanan Personel ID'si
        /// </summary>
        public Guid? userId { get; set;}
    }
}
