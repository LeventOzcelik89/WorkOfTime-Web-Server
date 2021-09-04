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
        public CSM_StudentDepartment[] GetCSM_StudentDepartmentByStudentId(Guid studentId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CSM_StudentDepartment>().Where(a => a.studentId == studentId).Execute().ToArray();
            }
        }
    }
}
