using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İzin, görevlendirme vs. süreci bulunan işlemlerin kurallarının personellerle ilişkilendirildiği tablodur.
    /// </summary>
    public partial class UT_RulesUser : InfolineTable
    {
        /// <summary>
        /// Kuralın tutulduğu alandır.
        /// </summary>
        public Guid? rulesId { get; set;}
        /// <summary>
        /// Kullanıcının tutulduğu alandır.
        /// </summary>
        public Guid? userId { get; set;}
    }
}
