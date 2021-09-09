using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_CompanyBasedPriceDetail : InfolineTable
    {
        /// <summary>
        /// Companybasedprice tablosunun ilişkisi
        /// </summary>
        public Guid companyBasedPriceId { get; set;}
        /// <summary>
        /// minimum miktar veya satış fiyatı
        /// </summary>
        public double? minCondition { get; set;}
        /// <summary>
        /// Oran, Fiyat
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Iskonto Oranı
        /// </summary>
        public double? discount { get; set;}
        /// <summary>
        /// Vade/Taksit Ay Miktarı
        /// </summary>
        public int? monthCount { get; set;}
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? endDate { get; set;}
        public double? price { get; set;}
    }
}
