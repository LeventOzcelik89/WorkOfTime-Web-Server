using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_Problem : InfolineTable
    {
        public string name { get; set;}
        public string code { get; set;}
        public string description { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
