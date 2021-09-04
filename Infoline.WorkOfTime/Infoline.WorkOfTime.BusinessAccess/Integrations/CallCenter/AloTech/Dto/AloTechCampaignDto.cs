using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.AloTech.Dto
{
    public class AloTechCampaignDto
    {
        public string campaignkey { get; set; }
        public bool status { get; set; }
        public string campaigntype { get; set; }
        public int isReady { get; set; }
        public string campaignname { get; set; }
    }

    public class AloTechCampaignsDto
    {
        public List<AloTechCampaignDto> CampaignList { get; set; }
    }



}
