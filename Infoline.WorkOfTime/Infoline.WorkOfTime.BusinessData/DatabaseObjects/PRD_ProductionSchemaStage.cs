using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretim Şema Aşamaları Tablosu
    /// </summary>
    public partial class PRD_ProductionSchemaStage : InfolineTable
    {
        /// <summary>
        /// Aşama Kodu
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Aşama Adı
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Aşama Sırası
        /// </summary>
        public int? stageNum { get; set;}
        /// <summary>
        /// Üretim Şeması Kaydı
        /// </summary>
        public Guid? productionSchemaId { get; set;}
    }
}
