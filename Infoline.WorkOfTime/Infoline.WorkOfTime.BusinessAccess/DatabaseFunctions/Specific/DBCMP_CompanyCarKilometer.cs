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
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public CMP_CompanyCarKilometer GetCMP_CompanyCarKilometerByMaxKilometer(Guid companyCarId,  DateTime entryDate)
        {
            using (var db = GetDB())

            {
                return db.Table<CMP_CompanyCarKilometer>().Where(a => a.companyCarId == companyCarId && a.entryDate <= entryDate ).OrderByDesc(a => a.kilometer).Execute().FirstOrDefault();
            }
        }
        public CMP_CompanyCarKilometer GetCMP_CompanyCarKilometerByMinKilometer(Guid companyCarId, DateTime entryDate)
        {
            using (var db = GetDB())

            {
                return db.Table<CMP_CompanyCarKilometer>().Where(a => a.companyCarId == companyCarId && a.entryDate >= entryDate).OrderBy(a => a.kilometer).Execute().FirstOrDefault();
            }
        }

        public CMP_CompanyCarKilometer GetCMP_CompanyCarKilometerByMaxStartKm(Guid? companyCarId)
        {
            using (var db = GetDB())

            {
                return db.Table<CMP_CompanyCarKilometer>().Where(a => a.companyCarId == companyCarId).OrderByDesc(a => a.kilometer).Execute().FirstOrDefault();
            }
        }
        public CMP_CompanyCarKilometer GetCMP_CompanyCarKilometerByMinStartKm(Guid companyCarId)
        {
            using (var db = GetDB())

            {
                return db.Table<CMP_CompanyCarKilometer>().Where(a => a.companyCarId == companyCarId).OrderBy(a => a.kilometer).Execute().FirstOrDefault();
            }
        }

    }
}
