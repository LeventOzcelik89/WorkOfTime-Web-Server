using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(UT_LocationConfigUser), "isTrackingActive")]
    public enum EnumUT_LocationConfigUserisTrackingActive
    {
        [Description("Hayır")]
        Evet = 0,
        [Description("Evet")]
        Hayir = 1
    }

    partial class WorkOfTimeDatabase
    {
        public UT_LocationConfigUser[] GetUT_LocationConfigUserByUserIdGetConfigIds(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_LocationConfigUser>().Where(x => x.userId == userId).Execute().ToArray();
            }
        }


        public UT_LocationConfigUser GetUT_LocationConfigUserAndConfigId(Guid userId,Guid locationConfigId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_LocationConfigUser>().Where(x => x.userId == userId && x.locationConfigId == locationConfigId).Execute().FirstOrDefault();
            }
        }

        public VWUT_LocationConfigUser GetVWUT_LocationConfigUserByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWUT_LocationConfigUser>().Where(x => x.userId == userId).Execute().OrderByDescending(x => x.created).FirstOrDefault();
            }
        }
    }
}
