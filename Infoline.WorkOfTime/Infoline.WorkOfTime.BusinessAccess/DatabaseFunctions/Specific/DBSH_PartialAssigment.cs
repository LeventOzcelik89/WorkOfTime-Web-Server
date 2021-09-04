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
        public string[] GetSH_PartialAssigmentStaffNameList(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_PartialAssigment>().Where(a => a.staffName != null && a.staffName != "").GroupBy(a => a.staffName).Select(a => new SH_PartialAssigment { staffName = a.Key }).Execute().Select(a => a.staffName).ToArray();
            }
        }

        public string[] GetSH_PartialAssigmentSchoolDepartmentList(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_PartialAssigment>().Where(a => a.schoolDepartment != null && a.schoolDepartment != "").GroupBy(a => a.schoolDepartment).Select(a => new SH_PartialAssigment { schoolDepartment = a.Key }).Execute().Select(a => a.schoolDepartment).ToArray();
            }
        }

    }
}
