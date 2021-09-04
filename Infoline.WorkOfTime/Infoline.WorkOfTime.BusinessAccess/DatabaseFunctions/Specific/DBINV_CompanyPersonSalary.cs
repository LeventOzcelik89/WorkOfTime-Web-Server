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

        public INV_CompanyPersonSalary GetINV_CompanyPersonSalaryByIdUser(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPersonSalary>().Where(x => x.IdUser == IdUser).OrderByDesc(x => x.EndDate).Take(1).Execute().FirstOrDefault();
            }
        }

        public INV_CompanyPersonSalary[] GetINV_CompanyPersonSalaryByIdUserAll(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPersonSalary>().Where(x => x.IdUser == IdUser).OrderByDesc(x => x.EndDate).Execute().ToArray();
            }
        }

        public INV_CompanyPersonSalary[] GETINV_CompanyPersonSalaryByIdUsers(Guid[] IdUsers, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPersonSalary>().Where(x => x.IdUser.In(IdUsers)).Execute().ToArray();
            }
        }
        public INV_CompanyPersonSalary GetINV_CompanyPersonSalaryBySalaryControl(Guid IdUser, DateTime? start, DateTime? end)
        {
            using (var db = GetDB())
            {
                return db.Table<INV_CompanyPersonSalary>().Where(a => a.IdUser == IdUser  &&
                 (((a.StartDate <= start && a.EndDate >= end) ||
                (a.StartDate < start && a.EndDate > start && a.EndDate < end) ||
                (a.StartDate > start && a.StartDate < end && a.EndDate > end) ||
                (start <= a.StartDate && end >= a.EndDate)
                ))).OrderByDesc(x => x.created).Execute().FirstOrDefault();
            }
        }


       
    }
}
