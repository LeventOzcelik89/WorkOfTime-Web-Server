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


    [EnumInfo(typeof(PDS_FormPermitTask), "status")]
    public enum EnumPDS_PDS_FormPermitTaskStatus
    {
        [Description("Aktif")]
        Active = 1,
        [Description("Pasif")]
        Passive = 0,
    }


    public enum EnumPDS_FormPermitTaskFilterType
    {
        [Description("Personel Bazlı")]
        Personel = 0,
        [Description("Departman Bazlı")]
        Department = 1,
        [Description("Pozisyon Bazlı")]
        Position = 2,
        [Description("Kendi")]
        Own = 3

    }


    partial class WorkOfTimeDatabase
    {
       


    }
}
