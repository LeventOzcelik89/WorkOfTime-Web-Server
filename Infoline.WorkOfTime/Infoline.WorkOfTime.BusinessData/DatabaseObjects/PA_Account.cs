using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Banka ve kasa hesap tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class PA_Account : InfolineTable
    {
        /// <summary>
        /// Hesap adının tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Hesap kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Hesap tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Döviz tipinin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? currencyId { get; set;}
        /// <summary>
        /// Bankanın idsinin tutulduğu alandır.
        /// </summary>
        public Guid? bankId { get; set;}
        /// <summary>
        /// Banka hesabının IBAN bilgisinin tutulduğu alandır.
        /// </summary>
        public string iban { get; set;}
        /// <summary>
        /// Bağlantılı olan tablonun idsinin tutulduğu alandır.
        /// </summary>
        public Guid? dataId { get; set;}
        /// <summary>
        /// Bağlantılı olan tablonun isminin tutulduğu alandır.
        /// </summary>
        public string dataTable { get; set;}
        /// <summary>
        /// Hesap durumunun tutulduğu alandır.
        /// </summary>
        public bool? status { get; set;}
        /// <summary>
        /// Hesabın varsayılan olup olmadığının tutulduğu alandır.
        /// </summary>
        public bool? isDefault { get; set;}
    }
}
