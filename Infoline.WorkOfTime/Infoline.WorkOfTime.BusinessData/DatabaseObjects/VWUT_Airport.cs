using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_Airport : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public short? status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
    }
}
