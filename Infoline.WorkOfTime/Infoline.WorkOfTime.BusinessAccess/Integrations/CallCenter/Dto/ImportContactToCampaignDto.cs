using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto
{
    public class ImportContactToCampaignDto
    {
        public string CampaignId { get; set; }
        public List<string> CampaignList { get; set; }
    }
}
