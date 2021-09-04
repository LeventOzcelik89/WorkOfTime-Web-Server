using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyPersonCalendar : InfolineTable
    {
        /// <summary>
        /// Takvim etkinliği başlangıç tarihi
        /// </summary>
        public DateTime? StartDate { get; set;}
        /// <summary>
        /// Takvim etkinliği bitiş tarihi
        /// </summary>
        public DateTime? EndDate { get; set;}
        /// <summary>
        /// Başlık
        /// </summary>
        public string Title { get; set;}
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description { get; set;}
        /// <summary>
        /// Enum : Toplantı, Not, Kendime Not
        /// </summary>
        public int? Type { get; set;}
        public bool? isSalary { get; set;}
    }
}
