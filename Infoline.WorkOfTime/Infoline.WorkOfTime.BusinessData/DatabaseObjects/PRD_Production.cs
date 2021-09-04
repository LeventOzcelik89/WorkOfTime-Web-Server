using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Üretim Tablosu
    /// </summary>
    public partial class PRD_Production : InfolineTable
    {
        public string name { get; set;}
        public string description { get; set;}
        /// <summary>
        /// Üretim Emri Tarihi
        /// </summary>
        public DateTime? productionOrderDate { get; set;}
        /// <summary>
        /// Planlanmış Üretim Tarihi
        /// </summary>
        public DateTime? scheduledProductionDate { get; set;}
        /// <summary>
        /// Üretim Yapılacak Deponun İşletmesi
        /// </summary>
        public Guid? productionCompanyId { get; set;}
        /// <summary>
        /// Üretim Yapılacak Depo
        /// </summary>
        public Guid? productionStorageId { get; set;}
        /// <summary>
        /// Merkez Depo İşletmesi
        /// </summary>
        public Guid? centerCompanyId { get; set;}
        /// <summary>
        /// Merkez İşletme Deposu
        /// </summary>
        public Guid? centerStorageId { get; set;}
        /// <summary>
        /// Ürün
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Üretilecek Adet
        /// </summary>
        public double? quantity { get; set;}
        /// <summary>
        /// Üretim Emri Kodu
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Beklenen Son Üretim Tarihi
        /// </summary>
        public DateTime? lastProductionDate { get; set;}
        /// <summary>
        /// Şema Adı
        /// </summary>
        public string schemaTitle { get; set;}
    }
}
