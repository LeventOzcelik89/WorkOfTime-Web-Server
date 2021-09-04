using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Uygulamayı çalıştıran Mobil Cihazların tutulduğu tablodur
    /// </summary>
    public partial class MB_MobileDevice : InfolineTable
    {
        /// <summary>
        /// Android için ANDROID_ID, IOS için ise VENDOR ID bilgisinin tutulduğu alandır
        /// </summary>
        public string UniqID { get; set;}
        /// <summary>
        /// Application ID
        /// </summary>
        public string AppID { get; set;}
        /// <summary>
        /// Application Version Code
        /// </summary>
        public string AppVersionCode { get; set;}
        /// <summary>
        /// Application Version Name
        /// </summary>
        public string AppVersionName { get; set;}
        /// <summary>
        /// Device
        /// </summary>
        public string Device { get; set;}
        /// <summary>
        /// Model
        /// </summary>
        public string Model { get; set;}
        /// <summary>
        /// Operator System Version
        /// </summary>
        public string OSVersion { get; set;}
        /// <summary>
        /// Brand
        /// </summary>
        public string Brand { get; set;}
        /// <summary>
        /// Board
        /// </summary>
        public string Board { get; set;}
        /// <summary>
        /// Product
        /// </summary>
        public string Product { get; set;}
        /// <summary>
        /// Serial No
        /// </summary>
        public string Serial { get; set;}
        /// <summary>
        /// SDK
        /// </summary>
        public string SDK { get; set;}
        /// <summary>
        /// Screen Scale
        /// </summary>
        public string ScreenScale { get; set;}
        /// <summary>
        /// ENUM => 0: Android 1: IOS
        /// </summary>
        public int? DeviceType { get; set;}
    }
}
