using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyPersonSalary : InfolineTable
    {
        /// <summary>
        /// Personel id sinin tutulduğu alandır
        /// </summary>
        public Guid? IdUser { get; set;}
        /// <summary>
        /// Personel maaş başlangıç tarihinin tutulduğu alandır
        /// </summary>
        public DateTime? StartDate { get; set;}
        /// <summary>
        /// Personel maaş bitiş tarihinin tutulduğu alandır
        /// </summary>
        public DateTime? EndDate { get; set;}
        /// <summary>
        /// Personel maaş bilgisinin tutulduğu alandır
        /// </summary>
        public double? Salary { get; set;}
    }
}
