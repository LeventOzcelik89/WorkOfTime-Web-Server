using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_ChangedDevice : InfolineTable
    {
        public Guid? oldInventoryId { get; set;}
        public Guid? newInventoryId { get; set;}
        public Guid? serviceId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string oldInventoryCode_Title { get; set;}
        public string newInventoryCode_Title { get; set;}
        public string serialCode { get; set;}
        public string oldproduct_Title { get; set;}
        public string newproduct_Title { get; set;}
        public string serviceCode { get; set;}
    }
}
