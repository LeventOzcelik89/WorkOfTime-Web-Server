using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Kısmi Zamanlı Görevlendirilmelerin Tutulduğu Tablodur.
    /// </summary>
    public partial class SH_PartialAssigment : InfolineTable
    {
        /// <summary>
        /// Görev başlangıç tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? startDate { get; set;}
        /// <summary>
        /// Görev bitiş tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? endDate { get; set;}
        /// <summary>
        /// Girmiş olduğu ders saatinin  tutulduğu alandır.
        /// </summary>
        public double? courseHours { get; set;}
        /// <summary>
        /// Derse girdiği bölümün tutulduğu alandır.
        /// </summary>
        public string schoolDepartment { get; set;}
        /// <summary>
        /// Girmiş olduğu dersin tutulduğu alandır.
        /// </summary>
        public string lesson { get; set;}
        /// <summary>
        /// Girmiş olduğu dersin saatlik ücretinin tutulduğu alandır.
        /// </summary>
        public double? hourlyWage { get; set;}
        public string staffName { get; set;}
    }
}
