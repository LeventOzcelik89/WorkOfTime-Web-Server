using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ödeme Planlarının Tutulduğu Tablodur.
    /// </summary>
    public partial class PA_Ledger : InfolineTable
    {
        /// <summary>
        /// İşlemin yapıldığı asıl hesabın tutulduğu alandır.
        /// </summary>
        public Guid? accountId { get; set;}
        /// <summary>
        /// İşlemin hangi hesapla yapıldığının tutulduğu alandır.
        /// </summary>
        public Guid? accountRealtedId { get; set;}
        /// <summary>
        /// İşlemin giriş-çıkış olması durumunun tutulduğu alandır.
        /// </summary>
        public short? direction { get; set;}
        /// <summary>
        /// Ödeme miktarının tutulduğu alandır.
        /// </summary>
        public double? amount { get; set;}
        /// <summary>
        /// Vergi miktarının tutulduğu alandır.
        /// </summary>
        public double? tax { get; set;}
        /// <summary>
        /// Ödeme yapıldığındaki güncel kurun tutulduğu alandır.
        /// </summary>
        public double? rateExchange { get; set;}
        /// <summary>
        /// Döviz tipinin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? currencyId { get; set;}
        /// <summary>
        /// Planlanan ödeme tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? date { get; set;}
        /// <summary>
        /// Ödeme tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// İşlemin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? transactionId { get; set;}
        /// <summary>
        /// Açıklamanın tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        public bool? isBalanceFixing { get; set;}
        public double? crossRate { get; set;}
        public Guid? advanceId { get; set;}
    }
}
