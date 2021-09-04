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
        /// <summary>
        /// VWINV_CompanyDepartments tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWINV_CompanyDepartments dizi objesini geri döndürür.</returns>
        public VWINV_CompanyDepartments[] GetVWINV_CompanyDepartmentsByRootId(Guid RootId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var rootDepartment = GetINV_CompanyDepartmentsById(RootId);
                return db.Table<VWINV_CompanyDepartments>()
                    .Where(a => a.ProjectId == rootDepartment.ProjectId && a.Type == rootDepartment.Type)
                    .OrderByDesc(a => a.created).Execute().ToArray();
            }
        }


        public VWINV_CompanyDepartments GetVWINV_CompanyDepartmentsByIdProjectBase(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyDepartments>()
                    .Where(a => a.ProjectId == projectId && a.PID == null)
                    .OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public VWINV_CompanyDepartments GetVWINV_CompanyDepartmentsByPidNullAndTypeOrganization(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyDepartments>().Where(c => c.Type == (int)EnumINV_CompanyDepartmentsType.Organization && c.PID == null).Execute().FirstOrDefault();
            }
        }
    }
}
