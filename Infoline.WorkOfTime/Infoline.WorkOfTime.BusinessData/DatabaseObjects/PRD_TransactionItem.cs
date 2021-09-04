using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Stok Hareketlerinin Tutulduğu Tablodur.
    /// </summary>
    public partial class PRD_TransactionItem : InfolineTable
    {
        /// <summary>
        /// İşlem idsinin tutulduğu alandır.
        /// </summary>
        public Guid? transactionId { get; set;}
        /// <summary>
        /// İşlem ürünün tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// İşlem miktarının tutulduğu alandır.
        /// </summary>
        public double? quantity { get; set;}
        /// <summary>
        /// İşlem birim tutarının tutulduğu alandır.
        /// </summary>
        public double? unitPrice { get; set;}
        /// <summary>
        /// işlemdeki serinumaralarının tutulduğu alandır.
        /// </summary>
        public string serialCodes { get; set;}
    }
}
