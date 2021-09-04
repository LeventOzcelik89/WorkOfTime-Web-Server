using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_PermitType : InfolineTable
    {
        public string Name { get; set;}
        /// <summary>
        /// Ücretli izin mi
        /// </summary>
        public bool? IsPaidPermit { get; set;}
        /// <summary>
        /// Ücretli izin ise gün sayısı
        /// </summary>
        public int? PaidPermitDay { get; set;}
        /// <summary>
        /// İzin saatlik olabilir mi ? Tüm Günlük mü yoksa
        /// </summary>
        public bool? CanHourly { get; set;}
        /// <summary>
        /// İzin ile alakalı açıklamalar...
        /// </summary>
        public string Descriptions { get; set;}
        /// <summary>
        /// Belgelendirilecek
        /// </summary>
        public bool? BeDocumented { get; set;}
        public bool? RequestStaff { get; set;}
        public bool? ViewStaff { get; set;}
    }
}
