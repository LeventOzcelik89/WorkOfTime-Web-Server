using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;
using System.Configuration;
using Infoline.PdksEntegrationApp.Utils;
using Infoline.PdksEntegrationApp.Models;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using System.Threading;
using System.IO;
using Infoline.Framework.Database;
using Infoline.PdksEntegrationApp.Devices;

namespace Infoline.PdksEntegrationApp.Devices.Models
{
    public class ZKTecoTK100_C : PdksDevice, IDisposable
    {
        public override short specifyLogStatus(LogInfo log)
        {
            switch (log.inOutMode)
            {
                case 0:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi;
                case 1:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti;
                case 4:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi;
                case 5:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti;
            }
            return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi;
        }
      
    }
}
