using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_Ticket 
    {
        public Guid id { get; set;}
        public Guid? userid { get; set;}
        public DateTime? createtime { get; set;}
        public DateTime? endtime { get; set;}
        public string IP { get; set;}
        public Guid? DeviceId { get; set;}
        public string User_Title { get; set;}
        public int? TotalMinute { get; set;}
        public string DeviceId_Title { get; set;}
    }
}
