using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_MonthlyTargetGroupedPersonPageReport 
    {
        public int TotalTarget { get; set;}
        public int TotalProduct { get; set;}
        public int TotalProductFocused { get; set;}
        public int TotalProductNotFocused { get; set;}
    }
}
