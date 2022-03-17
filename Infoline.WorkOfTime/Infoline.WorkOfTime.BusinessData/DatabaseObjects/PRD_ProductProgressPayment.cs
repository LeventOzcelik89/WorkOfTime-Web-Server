using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductProgressPayment : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? productId { get; set;}
        public string companyTypes { get; set;}
        public bool? existFTP { get; set;}
        public bool? isActivated { get; set;}
        public bool? isInventory { get; set;}
        public short? isProgressPayment { get; set;}
        public string imei { get; set;}
    }
}
