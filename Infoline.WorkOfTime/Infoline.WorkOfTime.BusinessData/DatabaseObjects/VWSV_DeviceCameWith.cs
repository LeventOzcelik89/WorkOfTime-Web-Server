using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_DeviceCameWith : InfolineTable
    {
        public Guid? productId { get; set;}
        public Guid? inventoryId { get; set;}
        public string description { get; set;}
        public int? amount { get; set;}
        public bool? hasLost { get; set;}
        public Guid? serviceId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string inventoryCode_Title { get; set;}
        public string productId_Title { get; set;}
        public string serviceId_Title { get; set;}
    }
}
