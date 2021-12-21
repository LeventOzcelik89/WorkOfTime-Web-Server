using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_DeviceProblem : InfolineTable
    {
        /// <summary>
        /// sv_problem tablosu karşılığı
        /// </summary>
        public Guid? problemTypeId { get; set;}
        /// <summary>
        /// sv_service alanı karşılığı
        /// </summary>
        public Guid? serviceId { get; set;}
        /// <summary>
        /// prd_product alanı karşılığı
        /// </summary>
        public Guid? productId { get; set;}
        public string serialNo { get; set;}
        /// <summary>
        /// prd_inventory alanı karşılığı
        /// </summary>
        public Guid? inventoryId { get; set;}
        public string description { get; set;}
    }
}
