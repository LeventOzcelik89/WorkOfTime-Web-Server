using System.Collections.Generic;

namespace Infoline.OmixEntegrationApp.TitanEntegration.Models
{
    public class DeviceResultList
    {
        public List<DeviceData> Data { get; set; }
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
    }
}
