using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Personel Talep Durumlarının tutulduğu tablodur.
    /// </summary>
    public partial class HR_StaffNeedsStatus : InfolineTable
    {
        /// <summary>
        /// Personel Talebi idsinin tutulduğu alandır.
        /// </summary>
        public Guid? staffNeedId { get; set;}
        /// <summary>
        /// Durum açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Durumun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
    }
}
