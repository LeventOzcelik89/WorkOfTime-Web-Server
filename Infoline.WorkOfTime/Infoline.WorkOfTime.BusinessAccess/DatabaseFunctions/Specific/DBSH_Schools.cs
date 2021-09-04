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
    [EnumInfo(typeof(SH_Schools), "Type")]
    public enum EnumSH_SchoolsType
    {
        [Description("Orta Öğretim")]
        OrtaOgretim = 0,
        [Description("Lise")]
        Lise = 1,
        [Description("Üniversite")]
        Universite = 2
    }
}
