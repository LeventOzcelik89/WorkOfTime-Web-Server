using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWMB_MobileDeviceRequest : InfolineTable
    {
        public Guid? DeviceId { get; set;}
        public Guid? TicketId { get; set;}
        public string Url { get; set;}
        public string Browser { get; set;}
        public int? TotalBytes { get; set;}
        public string PostedFiles { get; set;}
        public string IPAddress { get; set;}
        public string CreatedBy_Title { get; set;}
        public string ChangeBby_Title { get; set;}
        public string MobileDevice_Title { get; set;}
    }
}
