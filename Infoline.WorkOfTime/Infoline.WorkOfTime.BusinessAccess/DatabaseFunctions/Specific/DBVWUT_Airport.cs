using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(UT_Airport), "status")]
    public enum EnumUT_AirportStatus
    {
        [Description("Pasif")]
        passive = 0,
        [Description("Aktif")]
        active = 1,
    }
    public partial class WorkOfTimeDatabase
    {
    }
}