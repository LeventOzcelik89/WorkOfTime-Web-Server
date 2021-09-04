using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;


namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public VWINV_PermitOfficialPageReport GetVWINV_PermitOfficialSummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_PermitOfficialPageReport>().Execute().FirstOrDefault();
            }
        }

        public VWINV_PermitOffical[] GetVWINV_PermitOfficialByCalendar(DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_PermitOffical>().Where(c => c.StartDate >= start && c.EndDate <= end).Execute().ToArray();
            }
        }
    }
}