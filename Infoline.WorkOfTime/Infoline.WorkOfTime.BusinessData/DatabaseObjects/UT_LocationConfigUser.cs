using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Personel lokasyon takip tablosu
    /// </summary>
    public partial class UT_LocationConfigUser : InfolineTable
    {
        /// <summary>
        /// Sistem lokasyon ayar tanımları.
        /// </summary>
        public Guid? locationConfigId { get; set;}
        /// <summary>
        /// Kullanıcının id sinin tutulduğu tablodur.
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// Kullanıcının konum alınıp alınmadığını bildiren tablo.
        /// </summary>
        public bool? isTrackingActive { get; set;}
    }
}
