using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İşletme personellerinin işletmede görev aldıkları pozisyonların tutulduğu tablodur.
    /// </summary>
    public partial class INV_CompanyPersonDepartments : InfolineTable
    {
        /// <summary>
        /// Görev başlangıç tarihi
        /// </summary>
        public DateTime? StartDate { get; set;}
        /// <summary>
        /// Görev bitiş tarihi
        /// </summary>
        public DateTime? EndDate { get; set;}
        public string Title { get; set;}
        public Guid? DepartmentId { get; set;}
        /// <summary>
        /// Asıl Görevli olduğu pozisyon. ( Vekaleten oldukları 0, Asıl ise 1 )
        /// </summary>
        public bool? IsBasePosition { get; set;}
        public Guid? IdUser { get; set;}
        /// <summary>
        /// Personelin OGS 1. Yöneticisini Gösterir
        /// </summary>
        public Guid? Manager1 { get; set;}
        public Guid? Manager2 { get; set;}
        public Guid? Manager3 { get; set;}
        public Guid? Manager4 { get; set;}
        public Guid? Manager5 { get; set;}
        public Guid? Manager6 { get; set;}
    }
}
