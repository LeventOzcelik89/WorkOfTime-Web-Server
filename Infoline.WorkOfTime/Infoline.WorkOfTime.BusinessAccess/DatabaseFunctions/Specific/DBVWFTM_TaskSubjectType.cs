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

        public VWFTM_TaskSubjectType[] GetVWFTM_TaskSubjectByTaskId(Guid taskId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_TaskSubjectType>().Where(a => a.taskId == taskId && a.subjectId != null).Execute().ToArray();
            }
        }

    }
}
