using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_Problem : InfolineTable
    {
        /// <summary>
        /// Problem Adı
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Problem Kodu
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// problem alanı açıklaması
        /// </summary>
        public string description { get; set;}
    }
}
