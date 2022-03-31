using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

   
    public partial class WorkOfTimeDatabase
    {
        public SH_ShiftTrackingDeviceUsers GetSH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(Guid deviceId, String deviceUserId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTrackingDeviceUsers>().Where(a => a.deviceId == deviceId && a.deviceUserId == deviceUserId).Execute().FirstOrDefault();
            }
        }

        public SH_ShiftTrackingDeviceUsers[] GetSH_ShiftTrackingDeviceUsersByDeviceId(Guid deviceId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTrackingDeviceUsers>().Where(a => a.deviceId == deviceId).Execute().ToArray();
            }
        }
    }
}
