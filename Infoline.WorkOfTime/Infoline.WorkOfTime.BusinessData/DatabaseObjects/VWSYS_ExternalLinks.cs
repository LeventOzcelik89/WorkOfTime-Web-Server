using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSYS_ExternalLinks : InfolineTable
    {
        public string Url { get; set;}
        public string Label { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
