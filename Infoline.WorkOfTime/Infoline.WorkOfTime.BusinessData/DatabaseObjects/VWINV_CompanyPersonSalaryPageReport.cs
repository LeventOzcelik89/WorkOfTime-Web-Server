using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPersonSalaryPageReport 
    {
        public int ToplamMaasliPersonelSayisi { get; set;}
        public string EnYusekMaasliPersonel { get; set;}
        public double EnYusekMaas { get; set;}
        public string MaasTarihi { get; set;}
    }
}
