using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{

    partial class WorkOfTimeDatabase
    {

        public INV_CompanyPersonCalendarPersons[] GetINV_CompanyPersonCalendarPersonsByIdUser(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPersonCalendarPersons>().Where(a => a.IdUser == IdUser).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }
        public INV_CompanyPersonCalendarPersons[] INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(Guid IDPersonCalendar, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<INV_CompanyPersonCalendarPersons>("SELECT * FROM INV_CompanyPersonCalendarPersons Where IDPersonCalendar={0}", IDPersonCalendar).ToArray();
            }
        }


    }
}
