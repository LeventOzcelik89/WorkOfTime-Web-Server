using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{ 
     public partial class WorkOfTimeDatabase
    {

        public VWPRJ_Project[] GETVWPRJ_ProjectByProjectIds(Guid[] IdProjects,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_Project>().Where(x => x.id.In(IdProjects)).Execute().ToArray();
            }
        }


        public VWPRJ_Project[] GetVWPRJ_ProjectByCalendar(DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_Project>().Where(a => a.ProjectStartDate >= start && a.ProjectEndDate <= end && a.IsActive == true).Execute().ToArray();
            }
        }

        public VWPRJ_Project[] GetVWPRJ_ProjectApproachByLastSevenDay(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_Project>().Where(a => a.ProjectEndDate > DateTime.Now.Date.AddDays(day) && a.ProjectEndDate < DateTime.Now.Date.AddDays(6) && a.IsActive == true).Execute().ToArray();
            }
        }

        public VWPRJ_Project[] GetVWPRJ_ProjectApproachByEnd(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_Project>().Where(a => a.ProjectEndDate >= DateTime.Now.Date.AddDays(day) && a.ProjectEndDate <= DateTime.Now.Date.AddDays(1).AddSeconds(-1) && a.IsActive == true).Execute().ToArray();
            }
        }

        public VWPRJ_Project[] GetVWPRJ_ProjectByInsertToday(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPRJ_Project>().Where(a => a.created >= DateTime.Now.Date.AddDays(day) && a.created<= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }

    }



}