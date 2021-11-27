using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İşletme depolarının tutulduğu tablodur.
    /// </summary>
    public partial class CMP_Storage : InfolineTable
    {
        /// <summary>
        /// işletme idsinin tutulduğu alandır.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// Depo kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Depo isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Depo lokasyonunun tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
        /// <summary>
        /// Depo adresisnin tutulduğu alandır.
        /// </summary>
        public string address { get; set;}
        /// <summary>
        /// Depo adresi location idsinin tutulduğu alandır.
        /// </summary>
        public Guid? locationId { get; set;}
        /// <summary>
        /// Depo sorumlusu olan kulanıcının idsinin tutulduğu alandır.
        /// </summary>
        public Guid? supervisorId { get; set;}
        public string phone { get; set;}
        public string fax { get; set;}
        public Guid? pid { get; set;}
        public string email { get; set;}
        public string postCode { get; set;}
        public short? type { get; set;}
    }
}
