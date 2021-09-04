using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün CRM Satış Puan tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class PRD_ProductPointSelling : InfolineTable
    {
        /// <summary>
        /// Ürün Tanım id sinin tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Ürün puanının tutulduğu alandır.
        /// </summary>
        public double? point { get; set;}
        /// <summary>
        /// Ürün liste başlangıç tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Ürün liste bitiş tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? endDate { get; set;}
    }
}
