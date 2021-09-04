using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İşletme tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class CMP_Company : InfolineTable
    {
        /// <summary>
        /// İşletme tipinin tutulduğu alandır.
        /// </summary>
        public int? type { get; set;}
        /// <summary>
        /// Üst işletme idsinin tutulduğu alandır.
        /// </summary>
        public Guid? pid { get; set;}
        /// <summary>
        /// İşletme kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// İşletme isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// İşletme mersis numarasının tutulduğu alandır.
        /// </summary>
        public string mersisNo { get; set;}
        /// <summary>
        /// İşletme isminin tutulduğu alandır.
        /// </summary>
        public string email { get; set;}
        /// <summary>
        /// İşletme telefonun tutulduğu alandır.
        /// </summary>
        public string phone { get; set;}
        /// <summary>
        /// İşletme faks bilgisinin tutulduğu alandır.
        /// </summary>
        public string fax { get; set;}
        /// <summary>
        /// İşletme tipinin tutulduğu alandır.
        /// </summary>
        public short? taxType { get; set;}
        /// <summary>
        /// İşletme vergi dairesinin tutulduğu alandır.
        /// </summary>
        public string taxOffice { get; set;}
        /// <summary>
        /// İşletme vergi numarasının tutulduğu alandır.
        /// </summary>
        public string taxNumber { get; set;}
        /// <summary>
        /// İşletme lokasyonunun tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
        /// <summary>
        /// İşletme fatura adresisnin tutulduğu alandır.
        /// </summary>
        public string invoiceAddress { get; set;}
        /// <summary>
        /// İşletme fatura adresi ülke idsinin tutulduğu alandır.
        /// </summary>
        public Guid? invoiceAddressLocationId { get; set;}
        /// <summary>
        /// İşletme iletişim adresisnin tutulduğu alandır.
        /// </summary>
        public string openAddress { get; set;}
        /// <summary>
        /// İşletme iletişim adresi ülke idsinin tutulduğu alandır.
        /// </summary>
        public Guid? openAddressLocationId { get; set;}
        /// <summary>
        /// İşletme fatura ünvan bilgisinin tutulduğu alandır.
        /// </summary>
        public string commercialTitle { get; set;}
        public short? isActive { get; set;}
        public string description { get; set;}
    }
}
