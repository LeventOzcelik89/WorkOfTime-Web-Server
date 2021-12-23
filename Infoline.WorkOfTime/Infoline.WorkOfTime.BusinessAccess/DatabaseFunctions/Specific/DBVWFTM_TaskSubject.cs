using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWFTM_TaskSubject GetVWFTM_TaskSubjectByPid(Guid pid)
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_TaskSubject>().Where(a => a.pid == pid).Execute().FirstOrDefault();
            }
        }
    }
}
