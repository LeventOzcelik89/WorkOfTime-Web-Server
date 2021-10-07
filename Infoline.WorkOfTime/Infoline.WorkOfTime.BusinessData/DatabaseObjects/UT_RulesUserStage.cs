using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// İzin, görevlendirme vs. süreci bulunan işlemlerin kurallarının personellerle ilişkilendirildiği tablodur.
    /// </summary>
    public partial class UT_RulesUserStage : InfolineTable
    {
        /// <summary>
        /// Kuralın tutulduğu alandır.
        /// </summary>
        public Guid? rulesId { get; set;}
        /// <summary>
        /// Kural aşama sırasının tutulduğu alandır.
        /// </summary>
        public short? order { get; set;}
        /// <summary>
        /// Kural aşama tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Kullanıcının tutulduğu alandır.
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// Kural aşama başlığının tutulduğu alandır.
        /// </summary>
        public string title { get; set;}
        public Guid? roleId { get; set;}
        public double? limit { get; set;}
    }
}
