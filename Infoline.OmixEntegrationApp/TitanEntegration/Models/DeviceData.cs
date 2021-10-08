using System;
using System.Collections.Generic;

namespace Infoline.OmixEntegrationApp.TitanEntegration.Models
{
    public class DeviceData
    {
        public string Board { get; set; }
        public string Brand { get; set; }
        public string DeviceName { get; set; }
        public string Hardware { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Product { get; set; }
        public string Serial { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public DateTime Created { get; set; }
        public object Modified { get; set; }
        public string DeviceId { get; set; }
    }
}
