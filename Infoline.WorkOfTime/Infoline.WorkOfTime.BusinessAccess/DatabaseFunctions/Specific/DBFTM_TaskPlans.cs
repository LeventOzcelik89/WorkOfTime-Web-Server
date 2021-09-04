using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using System.ComponentModel;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(FTM_TaskPlan), "frequency")]
    public enum EnumFTM_TaskPlansFrequency
    {
        [Description("Günlük")]
        Gunluk = 1,
        [Description("Haftalık")]
        Haftalik = 2,
        [Description("Aylık")]
        Aylik = 3,
    }

    [EnumInfo(typeof(FTM_TaskPlan), "taskCreationTime")]
    public enum EnumFTM_TaskPlansTaskCreationTime
    {
        [Description("Hemen")]
        Hemen = 1,
        [Description("1 Ay Önce")]
        AyOnce = 2,
        [Description("1 Hafta Önce")]
        HaftaOnce = 3,
        [Description("1 Gün Önce")]
        GunOnce = 4,
        [Description("Görev Gününde")]
        Gununde = 5,
    }

    partial class WorkOfTimeDatabase
    {

    }
}
