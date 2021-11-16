using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_EntegrationStorage : InfolineTable
    {
        public Guid? EntegrationFileId { get; set;}
        /// <summary>
        /// Distribütörün Cari İd si
        /// </summary>
        public Guid? DistributorId { get; set;}
        /// <summary>
        /// Distribütör Adı
        /// </summary>
        public string DistributorName { get; set;}
        public Guid? DistStorageId { get; set;}
        /// <summary>
        /// Distribütör Depo Kodu
        /// </summary>
        public string DistStorageCode { get; set;}
        /// <summary>
        /// Distribütör Depo Adı
        /// </summary>
        public string DistStorageName { get; set;}
        /// <summary>
        /// Distribütör Depo Şehri
        /// </summary>
        public string DistStorageCity { get; set;}
        /// <summary>
        /// Distribütör Depo İlçesi
        /// </summary>
        public string DistStorageTown { get; set;}
        /// <summary>
        /// Sistemdeki Ürün Kodu
        /// </summary>
        public string ConsolidationCode { get; set;}
        /// <summary>
        /// Sistemdeki Ürün Adı
        /// </summary>
        public string ConsolidationName { get; set;}
        /// <summary>
        /// PRD_Product Tablosunun İd alanı tutulur. Zorunlu değildir, ConsalidateCode üzerinden bulunacaktır.
        /// </summary>
        public Guid? ProductId { get; set;}
        /// <summary>
        /// Inventory Tablosu id si bu alanda tutulur.
        /// </summary>
        public Guid? InventoryId { get; set;}
        /// <summary>
        /// Cihaz IMEI Numarası
        /// </summary>
        public string Imei { get; set;}
        /// <summary>
        /// Cihaz Seri No
        /// </summary>
        public string SerialNo { get; set;}
        /// <summary>
        /// Cihaz Adedi
        /// </summary>
        public int? Quantity { get; set;}
    }
}
