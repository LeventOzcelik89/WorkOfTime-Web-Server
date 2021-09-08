using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_CompanyBasedPrice : InfolineTable
    {
        /// <summary>
        /// Müşteri/Şirket Id'si
        /// </summary>
        public Guid companyId { get; set;}
        /// <summary>
        /// tüm ürünlere 0, tek ürüne 1 değeri döner
        /// </summary>
        public bool isSingle { get; set;}
        /// <summary>
        /// Genel,Satışa göre, adete göre mi olduğunu belirten alan
        /// </summary>
        public short toFilterSpecified { get; set;}
        /// <summary>
        /// minimum satış adedi
        /// </summary>
        public double? minAmount { get; set;}
        /// <summary>
        /// eğer 1 ise fiyata göre değilse iskontoya göre belirlenen alan
        /// </summary>
        public bool? isPrice { get; set;}
        /// <summary>
        /// taksit/vade miktarı
        /// </summary>
        public int? installmentAmount { get; set;}
        /// <summary>
        /// fiyat bilgisi
        /// </summary>
        public double? price { get; set;}
        /// <summary>
        /// ıskonto bilgisi
        /// </summary>
        public double? discount { get; set;}
        /// <summary>
        /// Ürün Id'si
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Vade,Taksit,Nakit olduğunu belirten alan
        /// </summary>
        public short? sellingType { get; set;}
    }
}
