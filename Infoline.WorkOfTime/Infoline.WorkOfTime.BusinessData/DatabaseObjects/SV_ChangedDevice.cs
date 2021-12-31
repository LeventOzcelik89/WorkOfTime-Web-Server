using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_ChangedDevice : InfolineTable
    {
        public Guid? oldInventoryId { get; set;}
        public Guid? newInventoryId { get; set;}
        /// <summary>
        /// servis alanı
        /// </summary>
        public Guid? serviceId { get; set;}
    }
}
