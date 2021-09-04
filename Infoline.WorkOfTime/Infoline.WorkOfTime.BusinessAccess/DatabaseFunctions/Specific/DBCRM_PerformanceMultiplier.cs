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

    [EnumInfo(typeof(CRM_PerformanceMultiplier), "TargetGroupType")]
    public enum EnumCRM_PerformanceMultiplierTargetGroupType
    {
        [Description("Genel Performans Çarpanı")]
        General = 0,
        [Description("Takım Lideri Performans Çarpanı")]
        TeamLeadder = 1,
        [Description("Satış Sorumlusu Performans Çarpanı")]
        SalesPerson = 2
    }



    partial class WorkOfTimeDatabase
    {

    }
}
