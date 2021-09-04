using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonCompetenciesPageReport 
    {
        public int ToplamPersonelYeterliligi { get; set;}
        public string SonEklenenPersonelYeterliligi { get; set;}
        public string EnÇokYeterliliğiBulunanKullanıcı { get; set;}
        public DateTime SonKayitTarihi { get; set;}
    }
}
