using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.BusinessAccess
{


    public partial class WorkOfTimeDatabase
    {

        public UserTimeStatus[] SH_TicketByMonthsTimeAlternate(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<UserTimeStatus>("SELECT CONVERT(NVARCHAR(7), createtime,126) DateMonthYear, Sum(DATEDIFF(HOUR,createtime ,endtime)) totalTime,count(distinct userid) kullanıcılar FROM SH_Ticket with(nolock) WHERE userid != '00000000-0000-0000-0000-000000000000' and createtime >= DATEADD(year, -1, GETDATE()) and not(MONTH(createtime) = MONTH(GETDATE()) and YEAR(createtime) != YEAR(GETDATE())) GROUP BY CONVERT(NVARCHAR(7), createtime, 126) ORDER BY CONVERT(NVARCHAR(7), createtime, 126)").ToArray();
            }
        }
        

    }


    public class UserTimeStatus
    {
        public string DateMonthYear { get; set; }
        public object kullanıcılar { get; set; }
        public object totalTime { get; set; }
    }

}
