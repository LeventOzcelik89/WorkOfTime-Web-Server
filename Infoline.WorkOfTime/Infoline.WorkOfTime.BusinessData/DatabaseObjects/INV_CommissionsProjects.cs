using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CommissionsProjects : InfolineTable
    {
        /// <summary>
        /// Görevlendirme Formu id IDCommissions alanı
        /// </summary>
        public Guid? IdCommissions { get; set;}
        /// <summary>
        /// Proje İd
        /// </summary>
        public Guid? IdProject { get; set;}
        public double? Percentile { get; set;}
    }
}
