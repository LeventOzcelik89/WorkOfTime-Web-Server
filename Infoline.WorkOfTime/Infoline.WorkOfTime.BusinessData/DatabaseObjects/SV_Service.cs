using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_Service : InfolineTable
    {
        public string code { get; set;}
        public Guid? inventoryId { get; set;}
        public string description { get; set;}
        public short? realStatus { get; set;}
        public short? deliveryType { get; set;}
        public string deliveryTypeDescription { get; set;}
        public Guid? cargoId { get; set;}
        public string cargoNo { get; set;}
        public bool? didComeFromCompany { get; set;}
        public Guid? companyId { get; set;}
        public short? customerType { get; set;}
        public string customerTypeDescription { get; set;}
    }
}
