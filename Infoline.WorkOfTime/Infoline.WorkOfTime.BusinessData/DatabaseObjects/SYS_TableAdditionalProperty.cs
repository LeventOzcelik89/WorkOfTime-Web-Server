using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Tabloların ek özelliklerinin tutulduğu tablodur.
    /// </summary>
    public partial class SYS_TableAdditionalProperty : InfolineTable
    {
        /// <summary>
        /// Kayıt tablosunun tutulduğu alandır.
        /// </summary>
        public string dataTable { get; set;}
        /// <summary>
        /// Kayıt idsinin tutulduğu alandır.
        /// </summary>
        public Guid? dataId { get; set;}
        /// <summary>
        /// Özellik isminin tutulduğu alandır.
        /// </summary>
        public string propertyKey { get; set;}
        /// <summary>
        /// Özellik açıklamasının tutulduğu alandır.
        /// </summary>
        public string propertyValue { get; set;}
    }
}
