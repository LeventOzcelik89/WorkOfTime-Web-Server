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
    partial class WorkOfTimeDatabase {

        public PDS_FormPermitTaskUser[] GetPDS_FormPermitTaskUserByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PDS_FormPermitTaskUser>().Where(a => a.formPermitTaskId == taskId).Execute().ToArray();
            }
        }

        public VWPDS_FormPermitTaskUser[] GetVWPDS_FormPermitTaskUserByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_FormPermitTaskUser>().Where(a => a.formPermitTaskId == taskId).Execute().ToArray();
            }
        }

        public VWPDS_FormPermitTaskUser[] GetVWPDS_FormPermitTaskUserByEvaluatorId(Guid evaluatorId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_FormPermitTaskUser>().Where(a => a.evaluatorId == evaluatorId).Execute().ToArray();
            }
        }

        public VWPDS_FormPermitTaskUser[] GetVWPDS_FormPermitTaskUserByTaskIds(Guid[] ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPDS_FormPermitTaskUser>().Where(a => a.formPermitTaskId.In(ids)).Execute().ToArray();
            }
        }

    }


}
