using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [BusinessAccess.EnumInfo(typeof(SH_Report), "type")]
    public enum EnumSH_ReportType
    {
        [Description("Kullanıcı detaylı rapor")]
        userDetailReport = 0,
    }
    public partial class WorkOfTimeDatabase
    {
  

    }
}
