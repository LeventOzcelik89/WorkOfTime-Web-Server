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

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public VWINV_CompanyPersonCalendar[] GetVWINV_CompanyNotificationsByToday()
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonCalendar>().Where(a => a.StartDate != null && a.EndDate != null && a.Type == (int)EnumINV_CompanyPersonCalendarType.Duyuru).Execute()
                    .Where(x => x.StartDate.Value.Date <= DateTime.Now.Date && x.EndDate.Value.Date >= DateTime.Now.Date).ToArray();
            }
        }

        public VWINV_CompanyPersonCalendar[] GetVWINV_CompanyPresentationByToday()
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonCalendar>().Where(a => a.StartDate != null && a.Type == (int)EnumINV_CompanyPersonCalendarType.Sunum).Execute()
                    .Where(x => x.StartDate.Value.Date <= DateTime.Now.Date && x.EndDate.Value.Date >= DateTime.Now.Date).ToArray();

            }
        }
        public VWINV_CompanyPersonCalendar[] GetVWINV_CompanyPersonNotificationByTodayInsert(int day, DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonCalendar>().Where(c => c.created >= DateTime.Now.Date.AddDays(day) && c.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)
                && c.Type == (int)EnumINV_CompanyPersonCalendarType.Duyuru).Execute().ToArray();
            }
        }

        public VWINV_CompanyPersonCalendar[] GetVWINV_CompanyPersonPresentationByTodayInsert(int day, DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonCalendar>().Where(c => c.created >= DateTime.Now.Date.AddDays(day) && c.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)
                && c.Type == (int)EnumINV_CompanyPersonCalendarType.Sunum).Execute().ToArray();
            }
        }
    }
}
