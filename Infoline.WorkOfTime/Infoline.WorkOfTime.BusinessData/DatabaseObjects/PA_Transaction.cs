using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ödeme Planlarının Tutulduğu Tablodur.
    /// </summary>
    public partial class PA_Transaction : InfolineTable
    {
        /// <summary>
        /// Faturanın idsinin tutulduğu alandır.
        /// </summary>
        public Guid? invoiceId { get; set;}
        /// <summary>
        /// Hesap idsinin tutulduğu alandır.
        /// </summary>
        public Guid? accountId { get; set;}
        /// <summary>
        /// İşlemin giriş-çıkış olması durumunun tutulduğu alandır.
        /// </summary>
        public short? direction { get; set;}
        /// <summary>
        /// İşlem tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Ödeme miktarının tutulduğu alandır.
        /// </summary>
        public double? amount { get; set;}
        /// <summary>
        /// Döviz tipinin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? currencyId { get; set;}
        /// <summary>
        /// Planlanan ödeme tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? date { get; set;}
        /// <summary>
        /// Açıklamanın tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Fatura tarihi, vergi dönemi gibi olayın gerçekleşme tarihi bilgilerinin tutulduğu alandır.
        /// </summary>
        public DateTime? progressDate { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public short? status { get; set;}
        public double? tax { get; set;}
    }
}
