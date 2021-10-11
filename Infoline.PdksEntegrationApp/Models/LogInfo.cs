using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp.Models
{
    public class LogInfo
    {
        public int MachineNumber { get; set; }
        public int UserDeviceId { get; set; }
        public DateTime DateTimeRecord { get; set; }
        public string logType { get; set; }
    }
    public enum LogType
    {
        Parmakİzi = 1,
        Sifre = 2
    }
}
