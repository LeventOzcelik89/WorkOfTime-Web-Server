using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [BusinessAccess.EnumInfo(typeof(PRJ_ProjectServiceLevel), "resolutionType")]
    public enum EnumPRJ_ProjectServiceLevelResolutionType
    {
        [Description("Gün")]
        Gun = 0,
        [Description("Saat")]
        Saat = 1,
    }


    partial class WorkOfTimeDatabase
    {
        public VWPRJ_ProjectServiceLevel[] GetVWPRJ_ProjectServiceLevelByProjectId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectServiceLevel>().Where(a => a.projectId == projectId).Execute().ToArray();
            }
        }
    }
}
