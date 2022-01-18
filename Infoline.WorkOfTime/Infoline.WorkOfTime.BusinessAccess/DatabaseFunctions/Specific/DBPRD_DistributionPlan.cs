using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [BusinessAccess.EnumInfo(typeof(PRD_DistributionPlan), "status")]
    public enum EnumPRD_DistributionPlanStatus
    {
        [Description("Dağıtım Planlandı"), Generic("icon", "icon-spinner", "color", "warning")]
        Planlandi = 0,
        [Description("Dağıtım Tamamlandı"), Generic("icon", "icon-ok-circle-1", "color", "primary")]
        Tamamlandi = 1,
        [Description("Dağıtım İptal Edildi"), Generic("icon", "icon-ok-circle-1", "color", "danger")]
        IptalEdildi = 2,
    }

    partial class WorkOfTimeDatabase
    {

    }
}
