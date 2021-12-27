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
        public CRM_OpponentCompany GetCRM_OpponentCompanyControl(string CompanyName)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_OpponentCompany>().Where(a => a.CompanyName == CompanyName).Execute().FirstOrDefault();
            }
        }

        public CRM_OpponentCompany GetCRM_OpponentCompanyUpdate(Guid id, string name)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_OpponentCompany>().Where(a => a.id != id && a.CompanyName == name).Execute().FirstOrDefault();
            }
        }

        public int GetCRM_OpponentCompanyByCompanyNameCount(String CompanyName, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CRM_OpponentCompany>().Where(a => a.CompanyName == CompanyName).Execute().Count();
            }
        }
    }
}
