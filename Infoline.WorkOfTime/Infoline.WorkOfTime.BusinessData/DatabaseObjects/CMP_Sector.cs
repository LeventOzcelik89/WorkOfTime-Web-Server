using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İşletme Sektör tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class CMP_Sector : InfolineTable
    {
        /// <summary>
        /// İşletme idsinin tutulduğu alandır.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// sektör id sinin tutulduğu alandır.
        /// </summary>
        public Guid? sectorId { get; set;}
    }
}
