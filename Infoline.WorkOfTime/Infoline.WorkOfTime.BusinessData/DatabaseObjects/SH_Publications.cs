using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Personel Yayınlarının Tutulduğu Tablodur.
    /// </summary>
    public partial class SH_Publications : InfolineTable
    {
        /// <summary>
        /// Yayın isim bilgisinin tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Yayın Açıklamasının  tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Yayın tarih bilgisinin  tutulduğu alandır.
        /// </summary>
        public DateTime? date { get; set;}
        /// <summary>
        /// Anaktar kelimelerinin tutulduğu alandır.
        /// </summary>
        public string keywords { get; set;}
    }
}
