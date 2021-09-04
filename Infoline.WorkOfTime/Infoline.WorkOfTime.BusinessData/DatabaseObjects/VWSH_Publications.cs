using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_Publications : InfolineTable
    {
        public string name { get; set;}
        public string description { get; set;}
        public DateTime? date { get; set;}
        public string keywords { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
