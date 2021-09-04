using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectTimelinePageReport 
    {
        public int ToplamCizelge { get; set;}
        public int ToplamIsPlani { get; set;}
        public int ToplamHakedis { get; set;}
        public string SonEklenenCizelge { get; set;}
    }
}
