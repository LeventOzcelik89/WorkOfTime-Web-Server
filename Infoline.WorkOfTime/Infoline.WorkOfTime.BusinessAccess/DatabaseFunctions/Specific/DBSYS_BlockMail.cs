using System.ComponentModel;
using Infoline.WorkOfTime.BusinessData;
using System.Data.Common;
using System;
using System.Linq;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public SYS_BlockMail[] GetSYS_BlockMailByUserId(Guid Id ,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_BlockMail>().Where(x => x.userId == Id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public SYS_BlockCalendar[] GetSYS_BlockCalendarByUserId(Guid Id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_BlockCalendar>().Where(x => x.userId == Id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }


        public SYS_BlockMail GetSYS_BlockMailByUserIdAndType(Guid Id, int type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_BlockMail>().Where(x => x.userId == Id && x.type == type).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public VWSYS_BlockMail GetVWSYS_BlockMailByMailAndType(string mail, int type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSYS_BlockMail>().Where(x => x.email == mail && x.type == type).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}
