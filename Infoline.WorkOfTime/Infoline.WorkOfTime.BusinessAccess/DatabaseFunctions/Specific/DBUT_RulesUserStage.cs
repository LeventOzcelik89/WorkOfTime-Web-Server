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
    [EnumInfo(typeof(UT_RulesUserStage), "type")]
    public enum EnumUT_RulesUserStage
    {
        [Description("1. Yönetici")]
        Manager1 = 0,
        [Description("2. Yönetici")]
        Manager2 = 1,
        [Description("3. Yönetici")]
        Manager3 = 2,
        [Description("4. Yönetici")]
        Manager4 = 3,
        [Description("5. Yönetici")]
        Manager5 = 4,
        [Description("6. Yönetici")]
        Manager6 = 5,
        [Description("Role Bağlı Seçim")]
        RoleBagliSecim = 10,
        [Description("Seçime Bağlı Kullanıcı")]
        SecimeBagliKullanici = 20,
        [Description("Son Onaylayacak Kullanıcı")]
        SonOnaylayici = 30

    }

    partial class WorkOfTimeDatabase
    {

    }
}
