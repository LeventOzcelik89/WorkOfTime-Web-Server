using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(MB_MobileDevice), "DeviceType")]
    public enum EnumMB_DeviceType
    {
        [Description("Android")]
        Android = 0,
        [Description("IOS")]
        IOS = 1,
    }
    public partial class WorkOfTimeDatabase
    {
        public MB_MobileDevice GetMB_MobileDeviceByDevice(MB_MobileDevice device, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<MB_MobileDevice>()
                    .Where(a =>
                        a.UniqID == device.UniqID &&
                        a.AppID == device.AppID &&
                        a.AppVersionCode == device.AppVersionCode &&
                        a.AppVersionName == device.AppVersionName &&
                        a.Device == device.Device &&
                        a.Model == device.Model &&
                        a.OSVersion == device.OSVersion &&
                        a.Brand == device.Brand &&
                        a.Board == device.Board &&
                        a.Product == device.Product &&
                        a.Serial == device.Serial &&
                        a.SDK == device.SDK &&
                        a.ScreenScale == device.ScreenScale &&
                        a.DeviceType == device.DeviceType
                    )
                    .OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}