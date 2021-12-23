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

        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsByIdUserAndType(Guid idUser, EnumINV_CompanyDepartmentsType type)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a =>
                    a.IdUser == idUser &&
                    a.OrganizationType == (Int32)type &&
                    a.IsBasePosition == true &&
                    a.StartDate < DateTime.Now &&
                    (a.EndDate == null || a.EndDate >= DateTime.Now)
                )
                .OrderBy(x => x.created).Execute().ToArray();
            }
        }
        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsByIdUserAllDepartmentsOfUser(Guid idUser)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a =>
                    a.IdUser == idUser &&
                    a.IsBasePosition == true &&
                    a.OrganizationType == 0
                )
                .OrderBy(x => x.created).Execute().ToArray();
            }
        }
        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsAllByIdUserAndType(Guid idUser, EnumINV_CompanyDepartmentsType type)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => a.IdUser == idUser && a.OrganizationType == (Int32)type)
                .OrderBy(x => x.created).Execute().ToArray();
            }
        }
        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsByIdProjectNotDateControl(Guid IdProject, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(x =>
                x.ProjectId == IdProject &&
                x.StartDate < DateTime.Now).Execute().ToArray();
            }
        }

        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsInDepartmentId(Guid[] departmentId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => a.DepartmentId.In(departmentId)).OrderBy(x => x.created).Execute().ToArray();
            }
        }


        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsUserManagerAndOrganization(Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => (a.IdUser == userId || a.Manager1 == userId || a.Manager2 == userId) && a.OrganizationType == (Int16)EnumINV_CompanyDepartmentsType.Organization).OrderBy(x => x.created).Execute().ToArray();
            }
        }


        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsActiveBasePositions(Guid idUser)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a =>
                a.IdUser == idUser &&
                a.IsBasePosition == true &&
                (a.EndDate == null || a.EndDate >= DateTime.Now)
                ).OrderBy(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsByIdUser(Guid idUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => a.IdUser == idUser && a.IsBasePosition == true).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CompanyPersonDepartments[] GetMyINV_CompanyPersonDepartments(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return
                    db.Table<VWINV_CompanyPersonDepartments>()
                        .Where(
                            a => a.StartDate < DateTime.Now && (a.EndDate == null || a.EndDate > DateTime.Now))
                        .OrderByDesc(x => x.created)
                        .Execute()
                        .ToArray();
            }
        }
        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => a.IsBasePosition == true).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CompanyPersonDepartments GetVWINV_CompanyPersonDepartmentsByUserIdAndEndDateNull(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(v => v.IdUser == id && v.EndDate == null).Execute().FirstOrDefault();
            }
        }


        public VWINV_CompanyPersonDepartments GetVWINV_CompanyPersonDepartmentsByUserIdAndEndDateNullIsBaseTrue(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(v => v.IdUser == id && v.EndDate == null && v.IsBasePosition == true && v.OrganizationType == (int)EnumINV_CompanyDepartmentsType.Organization).Execute().FirstOrDefault();
            }
        }

        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsProjects(Guid idUser)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a =>
                a.IdUser == idUser &&
                a.ProjectId != null &&
                (a.EndDate == null || a.EndDate >= DateTime.Now)
                ).OrderBy(x => x.created).Execute().ToArray();
            }
        }


        public VWINV_CompanyPersonDepartments[] GetVWINV_CompanyPersonDepartmentsByIdUserManager(Guid idUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonDepartments>().Where(a => (a.Manager1 == idUser || a.Manager2 == idUser) && a.IsBasePosition == true && a.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Organization && (a.EndDate == null || a.EndDate >= DateTime.Now) && (a.Person_Title != null && a.Person_Title != "")).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }
    }
}
