using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_PermitPageReport 
    {
        public double ToplamTalep { get; set;}
        public double Gelecek { get; set;}
        public double Gecmis { get; set;}
        public int IzinKullanan { get; set;}
    }
}
