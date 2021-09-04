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
 
        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimelineByIdProject(Guid idProject, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(w => w.IdProject == idProject).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimelineByCalendar(DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(c => c.StartDate >= start && c.StartDate <= end).Execute().ToArray();
            }
        }
        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimeLineApproachStartByLastSevenDay(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(c => c.StartDate > DateTime.Now.Date.AddDays(day) && c.StartDate < DateTime.Now.Date.AddDays(6)).Execute().ToArray();
            }
        }

        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimeLineByTodayStart(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(c => c.StartDate >= DateTime.Now.Date.AddDays(day) && c.StartDate <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }

        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimeLineApproachEndByLastSevenDay(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(d=>
                    d.EndDate != null).Execute().Where(c => 
                    c.EndDate.Value.Date > DateTime.Now.Date.AddDays(day) && c.EndDate < DateTime.Now.Date.AddDays(6)).ToArray();

                
            }
        }

        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimeLineByTodayEnd(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(c => c.EndDate >= DateTime.Now.Date.AddDays(day) && c.EndDate<= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }

        public VWPRJ_ProjectTimeline[] GetVWPRJ_ProjectTimeLineByTodayInsert(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_ProjectTimeline>().Where(c => c.created >= DateTime.Now.Date.AddDays(day) && c.created<= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }
    }
}
