using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_Invoice : InfolineTable
    {
        /// <summary>
        /// Alış-Satış olduğunu belirtir.
        /// </summary>
        public short? direction { get; set;}
        /// <summary>
        /// Faturanın tipini belirtir.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Faturaya girilen açıklamadır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Fatura seri numarası için kullanılır.
        /// </summary>
        public string serialNumber { get; set;}
        /// <summary>
        /// Fatura sıra numarası kaydedilir.
        /// </summary>
        public string rowNumber { get; set;}
        /// <summary>
        /// Tedarikçinin tutulduğu alandır.
        /// </summary>
        public Guid? supplierId { get; set;}
        /// <summary>
        /// Müşterinin tutulduğu alandır.
        /// </summary>
        public Guid? customerId { get; set;}
        /// <summary>
        /// Fatura veya teklifin döviz tipinin tutulduğu alandır.
        /// </summary>
        public Guid? currencyId { get; set;}
        /// <summary>
        /// Fatura kesim tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? issueDate { get; set;}
        /// <summary>
        /// Vade tarihi tutulur.
        /// </summary>
        public DateTime? expiryDate { get; set;}
        /// <summary>
        /// Stopaj yüzde olarak tutulur, genel toplama etki eder.
        /// </summary>
        public short? stopaj { get; set;}
        /// <summary>
        /// Ara toplam indiriminin tutulduğu alandır.
        /// </summary>
        public double? discount { get; set;}
        /// <summary>
        /// Ara toplam indiriminin tipinin tutulduğu alandır. (Yüzde ve Tutar)
        /// </summary>
        public short? discountType { get; set;}
        /// <summary>
        /// KDV üzerinden hesaplanan tevkifatın tutulduğu alandır. (Onda üzerinden)
        /// </summary>
        public double? tevkifat { get; set;}
        /// <summary>
        /// Sevk tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? sendingDate { get; set;}
        /// <summary>
        /// Durumun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Teklifin koşullarının tutuluduğu alandır.
        /// </summary>
        public string tenderConditions { get; set;}
        /// <summary>
        /// Ödeme tipinin tutulduğu alandır. (Peşin, Vadeli, Taksitli)
        /// </summary>
        public short? paymentType { get; set;}
        /// <summary>
        /// Taksit Sayısı
        /// </summary>
        public short? installmentCount { get; set;}
        /// <summary>
        /// Faturada seçilen kurun o anlık değerini belirtir.
        /// </summary>
        public double? rateExchange { get; set;}
        /// <summary>
        /// Müşterinin vergi numarsının tutulduğu alandır.
        /// </summary>
        public string customerTaxNumber { get; set;}
        /// <summary>
        /// Müşterinin vergi dairesinin tutulduğu alandır.
        /// </summary>
        public string customerTaxOffice { get; set;}
        /// <summary>
        /// Müşterinin fatura adresinin tutulduğu alandır.
        /// </summary>
        public string customerAdress { get; set;}
        /// <summary>
        /// Tedarikçinin vergi numarasının tutulduğu alandır.
        /// </summary>
        public string supplierTaxNumber { get; set;}
        /// <summary>
        /// Tedarikçinin vergi dairesinin tutulduğu alandır.
        /// </summary>
        public string supplierTaxOffice { get; set;}
        /// <summary>
        /// Tedarikçinin fatura adresinin tutulduğu alandır.
        /// </summary>
        public string supplierAdress { get; set;}
        /// <summary>
        /// Müşterinin ünvanının tutulduğu alandır.
        /// </summary>
        public string customerTitle { get; set;}
        /// <summary>
        /// Tedarikçinin ünvanının tutulduğu alandır.
        /// </summary>
        public string supplierTitle { get; set;}
        /// <summary>
        /// Müşteri deposunun tutulduğu alandır..
        /// </summary>
        public Guid? customerStorageId { get; set;}
        /// <summary>
        /// Tedarikçi deposunun tutulduğu alandır.
        /// </summary>
        public Guid? supplierStorageId { get; set;}
        /// <summary>
        /// Geçerlilik tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? validityDate { get; set;}
        public double? discountPercent { get; set;}
        public Guid? invoiceDocumentTemplateId { get; set;}
    }
}
