using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_TitanDeviceActivated : InfolineTable
    {
        public string SerialNumber { get; set;}
        public string IMEI1 { get; set;}
        public string IMEI2 { get; set;}
        public DateTime? CreatedOfTitan { get; set;}
        public Guid? DeviceId { get; set;}
        public Guid? ProductId { get; set;}
        public Guid? InventoryId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string InvetoryId_Code { get; set;}
    }
}
