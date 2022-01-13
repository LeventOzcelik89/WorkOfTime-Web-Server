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
  
        public VWINV_CompanyCalendar[] GetINV_CompanyMeetingByNamesToday(DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyCalendar>().Where(c => c.Start!= null && c.End != null && c.Type == (int)EnumINV_CompanyPersonCalendarType.Toplanti).Execute()
                    .Where(c=>c.Start.Value.Date<= DateTime.Now.Date &&  c.End.Value.Date >= DateTime.Now.Date).ToArray();
            }
        }

        public VWINV_CompanyCalendar[] GetVWINV_CompanyMeetingByTodayInsert(int day, DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyCalendar>().Where(c => c.created >= DateTime.Now.Date.AddDays(day) && c.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)  
                && c.Type == (int)EnumINV_CompanyPersonCalendarType.Toplanti).Execute().ToArray();
            }
        }
        public VWINV_CompanyCalendar[] VWINV_CompanyPersonCalendarDistinct(Guid? userId, DateTime start, DateTime end)
        {
            var text = "";
            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                text = " (PCP.IdUser ='" + userId + "' or PCP.createdby = '" + userId + "' or PCP.IdUser='"+new Guid("75d8d82f-add2-41dc-9920-92f6292efed4") +"') and";
            }
            using (var db = GetDB())
            {
                return db.ExecuteReader<VWINV_CompanyCalendar>("SELECT distinct PC.[id],PC.[created],PC.[createdby],PC.[StartDate] AS[Start],PC.[EndDate] AS[End],PC.[Title] AS[Title],PC.[Description],STUFF((SELECT ',' + firstname + ' ' + lastname From SH_User with(nolock) Where id in (Select IdUser From INV_CompanyPersonCalendarPersons with(nolock) Where IDPersonCalendar = (Select id FRom INV_CompanyPersonCalendar with(nolock)  Where id = PC.id)) FOR XML PATH('') ),1,1,'') AS[Katilimcilar],PC.[Type] AS[Type] FROM INV_CompanyPersonCalendar  as PC INNER JOIN INV_CompanyPersonCalendarPersons AS PCP WITH(NOLOCK) ON PC.id = PCP.IDPersonCalendar where " + text + " PC.[StartDate] >= {0} and PC.[EndDate] <= {1}", start, end).ToArray();
            }
        }


    }
}
