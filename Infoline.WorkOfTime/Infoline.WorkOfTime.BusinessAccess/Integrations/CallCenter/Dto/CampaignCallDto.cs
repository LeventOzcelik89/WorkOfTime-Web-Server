using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto
{


    public enum EnumReasonCode
    {
        Ulasildi = 0,
        Randevu = 1,
        Ulasilamadi = 2,
        Hatali_Numara = 3
    }

    public class CampaignCallDto
    {
        public string DialId { get; set; } 
        public EnumReasonCode ReasonCode { get; set; }
        public string FinishCode { get; set; }
        public bool IsSuccess { get; set; }
        public string AgentName { get; set; }
        public DateTime CallbackTime { get; set; }
        public string CallbackPhone { get; set; }

    }
}
