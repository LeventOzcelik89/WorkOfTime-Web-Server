using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectTimeline : InfolineTable
    {
        /// <summary>
        /// Proje id sinin tutulduğu alandır
        /// </summary>
        public Guid IdProject { get; set;}
        /// <summary>
        /// Enum =>0: Kabul,1:Madde , 2 Diğer
        /// </summary>
        public int? Type { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string Name { get; set;}
        public string Description { get; set;}
        public short? Status { get; set;}
    }
}
