using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCSM_Department : InfolineTable
    {
        public string name { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
