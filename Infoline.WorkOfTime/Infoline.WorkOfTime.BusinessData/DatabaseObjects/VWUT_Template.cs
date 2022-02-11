using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_Template : InfolineTable
    {
        public string full_Title { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string type_Title { get; set;}
        public string code { get; set;}
        public string title { get; set;}
        public string template { get; set;}
        public short? status { get; set;}
        public short? type { get; set;}
    }
}
