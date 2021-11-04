using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp.Models
{
    public class UserInfo 
    {
        public int MachineNumber { get; set; }
        public string UserDeviceId { get; set; }
        public string Name { get; set; }
        public int FingerIndex { get; set; }
        public string TmpData { get; set; }
        public int Privelage { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public string iFlag { get; set; }
    }

    public enum UserType
    {
        Yonetici = 1,
        Kullanıcı = 0
    }
}
