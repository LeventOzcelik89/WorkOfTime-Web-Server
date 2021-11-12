using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Entities
{
    public class SellIn
    {
        public string Dist { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string ConsolidationCode { get; set; }
        public string ConsolidationName { get; set; }
        public string Imei { get; set; }
        public string SeriNo { get; set; }
        public int Quantity { get; set; }
    }
}
