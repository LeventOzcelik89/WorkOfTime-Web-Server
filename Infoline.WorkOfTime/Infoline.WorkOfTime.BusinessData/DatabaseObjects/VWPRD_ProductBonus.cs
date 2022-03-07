using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductBonus : InfolineTable
    {
        public string code { get; set;}
        public string ruleName { get; set;}
        public string query { get; set;}
        public double? present { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
