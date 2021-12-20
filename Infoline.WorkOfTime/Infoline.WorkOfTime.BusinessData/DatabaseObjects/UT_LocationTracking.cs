using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Konumun izlenmesi için kayıt aldığı tablo.
    /// </summary>
    public partial class UT_LocationTracking : InfolineTable
    {
        /// <summary>
        /// Kullanıcının id sinin tutulduğu tablodur.
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// Kullanıcının kullandığı cihazın ıd sinin tutulduğu alandır
        /// </summary>
        public Guid? deviceId { get; set;}
        /// <summary>
        /// Zamanın belirtildiği tablodur.
        /// </summary>
        public string timeStamp { get; set;}
        /// <summary>
        /// Lokasyonun tutulduğu tablodur.
        /// </summary>
        public IGeometry  location { get; set;}
    }
}
