using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPersonPageReport 
    {
        public string EnCokPersoneliOlanIsletme { get; set;}
        public int ToplamPersonel { get; set;}
        public string SonPersonel { get; set;}
        public int? PersonAge { get; set;}
        public int? PersonGender { get; set;}
        public int? AkilliKademe { get; set;}
        public int? AllKademe { get; set;}
        public int? PersonMilitary { get; set;}
        public int? PersonBlood { get; set;}
        public int? AnkaraPerson { get; set;}
        public int? IstanbulPerson { get; set;}
        public int? Son1YildaIstenAyrilan { get; set;}
    }
}
