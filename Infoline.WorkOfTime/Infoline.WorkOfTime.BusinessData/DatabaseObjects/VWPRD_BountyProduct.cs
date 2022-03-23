using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_BountyProduct : InfolineTable
    {
        public Guid? bountyId { get; set;}
        public Guid? productId { get; set;}
        public Guid? inventoryId { get; set;}
        public double? price { get; set;}
        public string imei { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
    }
}
