using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İşletme depolarının alt birimlerinin tutulduğu tablodur.
    /// </summary>
    public partial class CMP_StorageSection : InfolineTable
    {
        /// <summary>
        /// Üst bölümün idsinin tutulduğu alandır.
        /// </summary>
        public Guid? pid { get; set;}
        /// <summary>
        /// İşletme idsinin tutulduğu alandır.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// Depo idsinin tutulduğu alandır.
        /// </summary>
        public Guid? storageId { get; set;}
        /// <summary>
        /// Depo kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Depo isminin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
    }
}
