using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_UserPendingTask 
    {
        public int visibleMobile { get; set;}
        public string icon { get; set;}
        public string title { get; set;}
        public string process { get; set;}
        public string url { get; set;}
        public Guid? userid { get; set;}
        public int? count { get; set;}
    }
}
