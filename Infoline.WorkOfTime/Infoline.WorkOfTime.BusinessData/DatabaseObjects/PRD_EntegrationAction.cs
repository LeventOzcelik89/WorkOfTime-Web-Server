using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_EntegrationAction : InfolineTable
    {
        /// <summary>
        /// FTP Dosya Adı
        /// </summary>
        public string FileName { get; set;}
        /// <summary>
        /// Fatura Numarası
        /// </summary>
        public string InvoiceNumber { get; set;}
        /// <summary>
        /// Fatura Tablosunun ilişki idsidir. Buradaki Fatura id şayet sistemde mevcut ise buraya yazılır.
        /// </summary>
        public Guid? InvoiceId { get; set;}
        /// <summary>
        /// FTP Dosya Yükleme Tarihi
        /// </summary>
        public DateTime? DateInFtp { get; set;}
        /// <summary>
        /// Distribütörün Cari İd si
        /// </summary>
        public Guid? DistributorId { get; set;}
        /// <summary>
        /// Distribütör Adı
        /// </summary>
        public string DistributorName { get; set;}
        /// <summary>
        /// Bayi Kodu
        /// </summary>
        public string CustomerOperatorCode { get; set;}
        /// <summary>
        /// Bayi Adı
        /// </summary>
        public string CustomerOperatorName { get; set;}
        /// <summary>
        /// Cari Tablosundan Gelecek Kod Eşleştirilerek
        /// </summary>
        public Guid? CustomerOperatorId { get; set;}
        public string CustomerOperatorStorageCode { get; set;}
        /// <summary>
        /// Storage Id
        /// </summary>
        public Guid? CustomerOperatorStorageId { get; set;}
        public string CustomerOperatorStorageCity { get; set;}
        public string CustomerOperatorStorageTown { get; set;}
        /// <summary>
        /// Üretici Adı(İşletme Adı)
        /// </summary>
        public string BranchName { get; set;}
        /// <summary>
        /// Üretici Kodu (İşletme Kodu)
        /// </summary>
        public string BranchCode { get; set;}
        /// <summary>
        /// Bayi Vergi Numarası 
        /// </summary>
        public string TaxNumber { get; set;}
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
