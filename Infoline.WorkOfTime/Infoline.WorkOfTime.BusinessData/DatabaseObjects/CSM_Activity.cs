using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aktivitelerin tutulduğu tablodur.
    /// </summary>
    public partial class CSM_Activity : InfolineTable
    {
        /// <summary>
        /// Aktivite tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Öğrencinin tutulduğu alandır.
        /// </summary>
        public Guid? studentId { get; set;}
        /// <summary>
        /// Aktivite zamanının tutulduğu alandır.
        /// </summary>
        public DateTime? date { get; set;}
        /// <summary>
        /// Aktivite süresinin dakika cinsinden tutulduğu alandır.
        /// </summary>
        public int? duration { get; set;}
        /// <summary>
        /// Aktivite aşamasının tutulduğu alandır.
        /// </summary>
        public Guid? stageId { get; set;}
        /// <summary>
        /// Randevu tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? contactDate { get; set;}
        /// <summary>
        /// Aktivite açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
    }
}
