using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectPageReport 
    {
        public int ToplamProje { get; set;}
        public int AktifProjeler { get; set;}
        public string EnYakinBitisli { get; set;}
        public string SonProje { get; set;}
        public int AktifTeknokent { get; set;}
        public int AktifTubitak { get; set;}
        public int AktifTubitakTeknokent { get; set;}
    }
}
