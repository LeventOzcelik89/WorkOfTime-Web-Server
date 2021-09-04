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
        public Guid[] GetFTM_TaskTemplateSubjectTypeByTaskTemplateIdTypesIds(Guid taskTemplateId)
        {
            using (var db = GetDB())
            {
                return db.Table<FTM_TaskTemplateSubjectType>().Where(a => a.taskTemplateId == taskTemplateId && a.subjectId != null).Select(a => new FTM_TaskTemplateSubjectType { id = a.id }).Execute().Select(x => x.subjectId.Value).ToArray();
            }
        }

        public VWFTM_TaskSubjectType[] GetVWFTM_TaskSubjectTypeByCustomerIdAndTaskIds(Guid customerId, Guid[] taskIds)
        {
            using(var db= GetDB())
            {
                return db.Table<VWFTM_TaskSubjectType>().Where(x => x.customerId == customerId && x.taskId.In(taskIds)).Execute().ToArray();
            }
        }



        public VWFTM_TaskSubjectType[] GetVWFTM_TaskSubjectTypeByTaskIds(Guid[] taskIds)
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_TaskSubjectType>().Where(x => x.taskId.In(taskIds)).Execute().ToArray();
            }
        }


        public FTM_TaskTemplateSubjectType[] GetFTM_TaskTemplateSubjectTypeByTaskTemplateId(Guid taskTemplateId)
        {
            using (var db = GetDB())
            {
                return db.Table<FTM_TaskTemplateSubjectType>().Where(a => a.taskTemplateId == taskTemplateId).Execute().ToArray();
            }
        }

        public FTM_TaskTemplateSubjectType[] GetFTM_TaskTemplateSubjectTypesByTaskTemplateId(Guid taskTemplateId)
        {
            using (var db = GetDB())
            {
                return db.Table<FTM_TaskTemplateSubjectType>().Where(a => a.taskTemplateId == taskTemplateId && a.subjectId != null).Execute().ToArray();
            }
        }

        public FTM_TaskTemplateSubjectType GetFTM_TaskTemplateSubjectTypeByTypeId(Guid typeId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskTemplateSubjectType>().Where(a => a.subjectId == typeId).Execute().FirstOrDefault();
            }
        }

    }
}
