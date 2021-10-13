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
        public VWCMP_Company GetVWCMP_CompanyByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }
        public VWCMP_Company GetVWCMP_CompanyByNameOrCode(string name,string code ,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.name == name || a.code == code).Execute().FirstOrDefault();
            }
        }

        public VWCMP_Company[] GetVWCMP_CompanyByTypeOther()
        {
            using (var db = GetDB())
            {
                return db.Table<VWCMP_Company>().Where(x => x.type == (int)EnumCMP_CompanyType.Diger).Execute().ToArray();
            }
        }


        public VWCMP_Company[] GetVWCMP_CompanyByCreatedby(Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCMP_Company>().Where(x => x.createdby == userId && x.type == (int)EnumCMP_CompanyType.Diger).Execute().ToArray();
            }
        }

        public VWCMP_Company[] GetVWCMP_CompanyInsertCustomerByToday(int day, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.type == (int)EnumCMP_CompanyType.Diger && a.created >= DateTime.Now.Date.AddDays(day) &&
                                                        a.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }



        public VWCMP_Company[] GetVWCMP_CompanyMyCompanies(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Company>().Where(a => a.type == (int)EnumCMP_CompanyType.Benimisletmem).Execute().ToArray();
            }
        }

        public VWCMP_Company[] GetVWCMP_CompanyOtherCompanies(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Company>().Where(a => a.type == (int)EnumCMP_CompanyType.Diger).Execute().ToArray();
            }
        }
        public VWCMP_Company[] GetVWCMP_CompanyByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Company>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }

        public string[] GetVWCMP_CompanyEmails(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.email != null).Select(a => new VWCMP_Company { email = a.email }).Execute().Select(a => a.email).Distinct().ToArray();
            }
        }
	}
}
