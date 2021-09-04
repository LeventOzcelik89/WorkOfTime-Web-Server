using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
   
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
    }
}
