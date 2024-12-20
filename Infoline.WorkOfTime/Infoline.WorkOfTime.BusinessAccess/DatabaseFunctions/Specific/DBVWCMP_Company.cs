﻿using System;
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
        public VWCMP_Company GetVWCMP_CompanyByTaxNumberOrCode(string code, string taxNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => (a.taxNumber != null && a.code != null) && a.code == code || a.taxNumber == taxNumber).Execute().FirstOrDefault();
            }
        }
        public VWCMP_Company GetVWCMP_CompanyByTaxNumber(string taxNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.taxNumber == taxNumber).Execute().FirstOrDefault();
            }
        }

        public CMP_Company GetCMP_CompanyByTaxNumber(string taxNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Company>().Where(a => a.taxNumber == taxNumber).Execute().FirstOrDefault();
            }
        }
        public VWCMP_Company GetCMP_CompanyByTaxNumberBayi(string taxNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.taxNumber == taxNumber && a.CMPTypes_Title.ToLower().Contains("bayi")).Execute().FirstOrDefault();
            }
        }
        public VWCMP_Company GetVWCMP_CompanyByNameOrCode(string name, string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Company>().Where(a => a.code == code || a.name == name).Execute().FirstOrDefault();
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

        public VWCMP_Company[] GetVWCMP_CompanyByCreatedbyAll(Guid[] userId, SimpleQuery simpleQuery)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCMP_Company>().ExecuteSimpleQuery(simpleQuery).Where(a => a.createdby.In(userId)).ToArray();
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
        public VWCMP_Company GetVWCMP_CompanyByIdDistributor(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Company>().Where(a => a.CMPTypes_Title.Contains("Distribütör") && a.id == id).Execute().FirstOrDefault();
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
