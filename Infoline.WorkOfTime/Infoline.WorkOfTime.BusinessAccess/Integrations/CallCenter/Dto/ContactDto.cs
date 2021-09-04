using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto
{



    public class ContactDto
    {
        public string UniqueId { get; set; }
        public string ListName { get; set; }
        public string CampaignId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? PlannedDate { get; set; } 
    }
}
