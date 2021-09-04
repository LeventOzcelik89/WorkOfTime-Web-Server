using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Stok Envanter İşlemlerinin Tutulduğu Tablodur.
    /// </summary>
    public partial class PRD_Transaction : InfolineTable
    {
        /// <summary>
        /// Kodunun tutulduğu alandır.
        /// </summary>
        public DateTime? date { get; set;}
        /// <summary>
        /// Kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Durumunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Açıklamanın tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Fatura  idsinin tutulduğu alandır.
        /// </summary>
        public Guid? invoiceId { get; set;}
        /// <summary>
        /// Sipariş idsinin tutulduğu alandır.
        /// </summary>
        public Guid? orderId { get; set;}
        /// <summary>
        /// Çıkış yapılacak yerin detay bilgilerinin tutulduğu alandır.
        /// </summary>
        public Guid? outputId { get; set;}
        /// <summary>
        /// Çıkış yapılacak yerin tablosunun tutulduğu alandır.
        /// </summary>
        public string outputTable { get; set;}
        /// <summary>
        /// Çıkış yapılacak carinin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? outputCompanyId { get; set;}
        /// <summary>
        /// Giriş yapılacak yerin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? inputId { get; set;}
        /// <summary>
        /// Giriş yapılacak yerin tablosunun tutulduğu alandır.
        /// </summary>
        public string inputTable { get; set;}
        /// <summary>
        /// Giriş yapılacak carinin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? inputCompanyId { get; set;}
        /// <summary>
        /// Başlangıç tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Bitiş tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? endDate { get; set;}
    }
}
