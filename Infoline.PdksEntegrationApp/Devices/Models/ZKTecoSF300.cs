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
    public class ZKTecoSF300 : PdksDevice, IDisposable
    {          
        public override short specifyLogStatus(LogInfo log)
        {
            return (log.DateTimeRecord.Hour >= 4 && log.DateTimeRecord.Hour <= 12) == true ? (short)0 : (short)1;
        }

    }
}
