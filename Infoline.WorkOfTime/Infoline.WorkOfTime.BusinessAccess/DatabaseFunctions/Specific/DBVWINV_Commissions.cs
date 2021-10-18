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
    public partial class WorkOfTimeDatabase
    {
        public VWINV_Commissions[] GetVWINV_CommissionsByYear(DateTime date)
        {
            using (var db = GetDB())
            {
                var firstDate = new DateTime(date.Year, 1, 1);
                var lastDate = new DateTime(date.Year, 12, 31, 23, 59, 59);
                return db.Table<VWINV_Commissions>().Where(x => x.StartDate >= firstDate && x.StartDate <= lastDate).OrderBy(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_Commissions[] GetVWINV_CommissionsRequestByToday(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_Commissions>().Where(a => a.created >= DateTime.Now.Date.AddMonths(-1) && a.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }
        public VWINV_Commissions[] GetVWINV_CommissionsWaitingByAccept(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_Commissions>().Where(a => a.ApproveStatus == (int)EnumINV_CommissionsApproveStatus.Bekliyor).Execute().ToArray();
            }
        }

        public VWINV_Commissions[] GetVWINV_CommissionsPersonByToday(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_Commissions>().Where(a => a.StartDate != null && a.EndDate != null).Execute()
                        .Where(x => x.StartDate.Value.Date <= DateTime.Now.Date && x.EndDate.Value.Date >= DateTime.Now.Date).ToArray();
            }
        }

    }
}
