using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_PresentationProducts : InfolineTable
    {
        public Guid? PresentationId { get; set;}
        public Guid? ProductId { get; set;}
        /// <summary>
        /// Miktar
        /// </summary>
        public int? Amount { get; set;}
        /// <summary>
        /// Yeni Taşıma mı
        /// </summary>
        public bool? IsNew { get; set;}
    }
}
