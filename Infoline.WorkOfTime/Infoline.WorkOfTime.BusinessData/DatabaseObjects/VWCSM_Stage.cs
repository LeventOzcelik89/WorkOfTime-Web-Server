using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCSM_Stage : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public string color { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
    }
}
