using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Öğrencilerin ilgilendiği bölümlerin tutulduğu tablodur.
    /// </summary>
    public partial class CSM_StudentDepartment : InfolineTable
    {
        /// <summary>
        /// Öğrencinin tutulduğu alandır.
        /// </summary>
        public Guid? studentId { get; set;}
        /// <summary>
        /// Bölüm bilgisinin tutulduğu alandır.
        /// </summary>
        public Guid? departmentId { get; set;}
    }
}
