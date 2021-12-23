using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_ServiceOperation : InfolineTable
    {
        /// <summary>
        /// servis numarası
        /// </summary>
        public Guid? serviceId { get; set;}
        /// <summary>
        /// durum
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// açıklama
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// operasyon yapan
        /// </summary>
        public Guid? userId { get; set;}
    }
}
