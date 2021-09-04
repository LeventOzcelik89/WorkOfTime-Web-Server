using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWMB_MobileDevice : InfolineTable
    {
        public string UniqID { get; set;}
        public string AppID { get; set;}
        public string AppVersionCode { get; set;}
        public string AppVersionName { get; set;}
        public string Device { get; set;}
        public string Model { get; set;}
        public string OSVersion { get; set;}
        public string Brand { get; set;}
        public string Board { get; set;}
        public string Product { get; set;}
        public string Serial { get; set;}
        public string SDK { get; set;}
        public string ScreenScale { get; set;}
        public int? DeviceType { get; set;}
        public string CreatedBy_Title { get; set;}
        public string ChangeBby_Title { get; set;}
        public string DeviceType_Title { get; set;}
    }
}
