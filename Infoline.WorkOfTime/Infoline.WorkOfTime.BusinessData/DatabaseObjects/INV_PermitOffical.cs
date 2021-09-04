using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_PermitOffical : InfolineTable
    {
        /// <summary>
        /// İzin Tipleri = Ramazan Bayramı, Kurban Bayramı, Resmi Tatil ( 23 Nisan vb. )
        /// </summary>
        public int? Type { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
    }
}
