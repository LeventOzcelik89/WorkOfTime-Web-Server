using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.AloTech.Dto
{
    public class AloTechResultDto
    {
        public string message { get; set; }
        public bool success { get; set; }
        public bool phone_conflict { get; set; }
    }
}
