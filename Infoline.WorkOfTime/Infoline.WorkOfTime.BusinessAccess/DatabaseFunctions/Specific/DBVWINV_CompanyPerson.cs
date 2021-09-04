using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public VWINV_CompanyPerson GetVWINV_CompanyPersonLastByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPerson>().Where(a => a.IdUser == userId).OrderByDesc(x => x.JobStartDate).Skip(0).Take(1).Execute().FirstOrDefault();
            }
        }


        public VWINV_CompanyPerson GetVWINV_CompanyPersonFirstByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPerson>().Where(a => a.IdUser == userId).OrderBy(x => x.JobStartDate).Skip(0).Take(1).Execute().FirstOrDefault();
            }
        }

        public VWINV_CompanyPerson[] GetVWINV_CompanyPersonByCompanyIdAndJobEndDateNull(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPerson>().Where(a => a.CompanyId == companyId && a.JobEndDate == null).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CompanyPerson[] GetVWINV_CompanyPersonByIdUser(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CompanyPerson>().Where(a => a.IdUser == IdUser).Execute().ToArray();
            }
        }
        public VWINV_CompanyPersonPageReport GetVWINV_CompanyPersonPageReportSummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonPageReport>().Execute().FirstOrDefault();
            }
        }

    }
}