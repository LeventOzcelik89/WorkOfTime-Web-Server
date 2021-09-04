using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    partial class WorkOfTimeDatabase
    {
        public VWUT_LocationTracking[] GetVWUT_LocationTrackingByUserIdAndDates(Guid userId, DateTime startDate, DateTime endDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWUT_LocationTracking>().Where(x => x.takenDate >= startDate && x.takenDate <= endDate && x.userId == userId).Execute().ToArray();
            }
        }


    }
}
