using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSRV_ServiceMaintenanceStepsPageReport 
    {
        public int ToplamBakimFormu { get; set;}
        public int KullanilanFormAdeti { get; set;}
        public string EnCokKullanilanBakimFormu { get; set;}
        public string EnAzKullanilanBakimFormu { get; set;}
    }
}
