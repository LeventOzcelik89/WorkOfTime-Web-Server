using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretim Kaydının Aşamalarının Tablosu
    /// </summary>
    public partial class PRD_ProductionStage : InfolineTable
    {
        /// <summary>
        /// Üretim Kaydı
        /// </summary>
        public Guid? productionId { get; set;}
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
        public int? stageNumber { get; set;}

		public Guid? productionSchemaId { get; set; }
	}
}
