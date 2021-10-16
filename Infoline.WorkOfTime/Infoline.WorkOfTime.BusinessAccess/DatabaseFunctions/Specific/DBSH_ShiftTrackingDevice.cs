using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

   
    public partial class WorkOfTimeDatabase
    {
        public SH_ShiftTrackingDevice[] GetSH_ShiftTrackingDeviceByBrandAndModel(String Brand, String Model)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTrackingDevice>().Where(a => a.DeviceBrand == Brand && a.DeviceModel == Model).Execute().ToArray();
            }
        }
    }
}
