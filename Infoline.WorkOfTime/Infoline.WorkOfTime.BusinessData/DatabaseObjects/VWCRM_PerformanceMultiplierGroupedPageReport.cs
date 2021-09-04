using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PerformanceMultiplierGroupedPageReport 
    {
        public int TotalMultiplier { get; set;}
        public int TotalProduct { get; set;}
        public int TotalProductFocused { get; set;}
        public int TotalProductNotFocused { get; set;}
    }
}
