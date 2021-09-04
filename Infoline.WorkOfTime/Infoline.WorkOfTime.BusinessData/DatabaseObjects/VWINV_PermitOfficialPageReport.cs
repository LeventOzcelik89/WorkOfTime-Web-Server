using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_PermitOfficialPageReport 
    {
        public int ToplamResmiİzinSayisi { get; set;}
        public int ToplamİzinSayisi { get; set;}
        public int ToplamİzinTipiSayisi { get; set;}
        public int? Gun { get; set;}
    }
}
