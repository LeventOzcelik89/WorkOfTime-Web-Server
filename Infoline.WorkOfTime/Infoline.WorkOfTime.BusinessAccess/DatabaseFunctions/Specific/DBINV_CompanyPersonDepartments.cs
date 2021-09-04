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

        public INV_CompanyPersonDepartments[] GetINV_CompanyPersonDepartmentsByIdUser(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPersonDepartments>().Where(a => a.IdUser == IdUser).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CompanyPersonDepartments[] GetINV_CompanyPersonDepartmentsByIdUserAndType(Guid IdUser, int type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => a.IdUser == IdUser && a.OrganizationType == type && a.IsBasePosition == true).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }


        public INV_CompanyPersonDepartments[] GetINV_CompanyPersonDepartmentsInDepartmentId(Guid[] departmentId)
        {
            using (var db = GetDB())
            {
                return db.Table<INV_CompanyPersonDepartments>().Where(a => a.DepartmentId.In(departmentId)).OrderBy(x => x.created).Execute().ToArray();
            }
        }

        public INV_CompanyPersonDepartments GetINV_CompanyPersonDepartmentsByDepartmentId(Guid departmentId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPersonDepartments>().Where(a => a.DepartmentId == departmentId).OrderByDesc(x => x.created).Execute().FirstOrDefault();
            }
        }
    }
}
