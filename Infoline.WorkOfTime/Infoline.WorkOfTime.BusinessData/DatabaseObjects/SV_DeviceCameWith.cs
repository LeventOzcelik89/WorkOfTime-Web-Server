using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_DeviceCameWith : InfolineTable
    {
        /// <summary>
        /// Ürün Alanı
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Açıklama
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Miktar
        /// </summary>
        public int? amount { get; set;}
        /// <summary>
        /// Yanında verilen parça kayıp mı oldu ? 
        /// </summary>
        public bool? hasLost { get; set;}
        /// <summary>
        /// servis alanı
        /// </summary>
        public Guid? serviceId { get; set;}
    }
}
