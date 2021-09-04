using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(INV_PermitConfirmation), "hasApprove")]
    public enum EnumINV_PermitConfirmationhasApprove
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1
    }
}
