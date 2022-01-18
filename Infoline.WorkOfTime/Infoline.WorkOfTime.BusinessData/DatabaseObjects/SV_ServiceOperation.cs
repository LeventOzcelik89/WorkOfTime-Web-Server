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
        public Guid? CargoId { get; set;}
        public Guid? CompanyId { get; set;}
        public string CargoNo { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public short? customerType { get; set;}
        public short? deliveryType { get; set;}
        public Guid? pid { get; set;}
    }
}
