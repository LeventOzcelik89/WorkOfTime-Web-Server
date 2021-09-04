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
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public Guid[] GetFTM_TaskSubjectTypeByTaskIdTypesIds(Guid taskId)
        {
            using (var db = GetDB())
            {
                return db.Table<FTM_TaskSubjectType>().Where(a => a.taskId == taskId && a.subjectId != null).Select(a => new FTM_TaskSubjectType { id = a.id }).Execute().Select(x => x.subjectId.Value).ToArray();
            }
        }
        public FTM_TaskSubjectType GetFTM_TaskSubjectTypeByTypeId(Guid typeId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskSubjectType>().Where(a => a.subjectId == typeId).Execute().FirstOrDefault();
            }
        }
        public FTM_TaskSubjectType[] GetFTM_TaskSubjectTypeByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskSubjectType>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }

        public FTM_TaskSubjectType[] GetFTM_TaskSubjectTypeByTaskIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskSubjectType>().Where(a => a.taskId.In(ids)).Execute().ToArray();
            }
        }
    }
}
