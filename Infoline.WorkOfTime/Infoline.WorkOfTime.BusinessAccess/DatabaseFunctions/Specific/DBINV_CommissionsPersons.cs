using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(INV_CommissionsPersons), "isOwner")]
    public enum EnumINV_CommissionsPersonsIsOwner
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1

    }
}
