using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class UT_Template : InfolineTable
    {
        /// <summary>
        /// Şablon Kodu
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Şablon Başlığı
        /// </summary>
        public string title { get; set;}
        /// <summary>
        /// Şablon İçeriği
        /// </summary>
        public string template { get; set;}
        /// <summary>
        /// Şablon Durumu
        /// </summary>
        public short? status { get; set;}
        public short? type { get; set;}
    }
}
