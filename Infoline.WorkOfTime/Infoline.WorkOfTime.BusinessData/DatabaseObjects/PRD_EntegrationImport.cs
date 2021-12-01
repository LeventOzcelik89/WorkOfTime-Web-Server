using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_EntegrationImport : InfolineTable
    {
        /// <summary>
        /// Bayi Adi
        /// </summary>
        public string customerName { get; set;}
        /// <summary>
        /// Bayi Kodu
        /// </summary>
        public string customerCode { get; set;}
        /// <summary>
        /// Cihaz Modeli
        /// </summary>
        public string productModel { get; set;}
        /// <summary>
        /// Distributor Adi
        /// </summary>
        public string distributorName { get; set;}
        /// <summary>
        /// Distributor Onay Tarihi
        /// </summary>
        public DateTime? distributorConfirmationDate { get; set;}
        /// <summary>
        /// Imei
        /// </summary>
        public string imei { get; set;}
        /// <summary>
        /// Müsteri Tipi
        /// </summary>
        public string customerType { get; set;}
        /// <summary>
        /// Sözlesme Baslangiç Tarihi
        /// </summary>
        public DateTime? contractStartDate { get; set;}
        public DateTime? contractEndDate { get; set;}
        /// <summary>
        /// Sözlesme Numarasi
        /// </summary>
        public string contractCode { get; set;}
        /// <summary>
        /// Ürün Grubu
        /// </summary>
        public string productGroup { get; set;}
        /// <summary>
        /// Satis Kanali Detayi
        /// </summary>
        public string sellingChannelType { get; set;}
        /// <summary>
        /// Distributor Kodu
        /// </summary>
        public string distributorCode { get; set;}
        /// <summary>
        /// Toplam Satis Adedi
        /// </summary>
        public int? sellingQuantity { get; set;}
        /// <summary>
        /// Ay
        /// </summary>
        public int? month { get; set;}
        /// <summary>
        /// Yıl
        /// </summary>
        public int? year { get; set;}
    }
}
