using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto
{
    public class CampaignDto
    {
        public string CampaingId { get; set; }
        public string CampaignName { get; set; }
        public bool? CampaignStatus { get; set; }
        public string CampaignType { get; set; }
        public int CampaignIsReady { get; set; }

    }
}
