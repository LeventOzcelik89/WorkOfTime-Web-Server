using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretim Şeması Tablosu
    /// </summary>
    public partial class PRD_ProductionSchema : InfolineTable
    {
        /// <summary>
        /// Şema Kodu
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Şema Adı
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Ürün
        /// </summary>
        public Guid? productId { get; set;}
    }
}
