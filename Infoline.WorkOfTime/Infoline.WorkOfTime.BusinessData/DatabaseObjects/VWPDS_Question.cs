using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPDS_Question : InfolineTable
    {
        public string question { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
