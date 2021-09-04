using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWMB_MobileDevicePageReport 
    {
        public int ToplamAndroidCihaz { get; set;}
        public int ToplamIOSCihaz { get; set;}
        public string EnCokKullanılanAndroidSurumu { get; set;}
        public string EnCokKullanılanIOSSurumu { get; set;}
    }
}
