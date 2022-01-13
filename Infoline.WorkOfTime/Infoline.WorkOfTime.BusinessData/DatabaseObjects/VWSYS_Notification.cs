using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSYS_Notification : InfolineTable
    {
        public Guid? userId { get; set;}
        public string message { get; set;}
        public string title { get; set;}
        public string url { get; set;}
        public string paramaters { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
    }
}
