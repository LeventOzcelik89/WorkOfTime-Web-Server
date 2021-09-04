using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPersonAvailabilityPageReport 
    {
        public int? ToplamMusaitlikSayisi { get; set;}
        public string EnCokMusaitlikPersoneli { get; set;}
        public string EnCokMusaitlikProjesi { get; set;}
        public string EnSonEklenenPersonel { get; set;}
    }
}
