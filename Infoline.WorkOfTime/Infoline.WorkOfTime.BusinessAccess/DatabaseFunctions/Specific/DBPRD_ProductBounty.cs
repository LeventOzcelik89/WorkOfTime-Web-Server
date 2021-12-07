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


        public PRD_ProductBounty[] GetPRD_ProductBountyByPeriod(int month, int year, Guid productId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductBounty>().Where(a => a.month == month && a.year == year && a.productId == productId).Execute().ToArray();
            }
        }

        public PRD_ProductBounty[] GetPRD_ProductBountiesByPeriodAndProductAndCompanyId(int month, int year, Guid productId, Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductBounty>().Where(a => a.month == month && a.year == year && a.productId == productId && a.companyId == companyId).Execute().ToArray();
            }
        }
        public PRD_ProductBounty GetPRD_ProductBountyByPeriodAndProductAndCompanyId(int month, int year, Guid productId, Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductBounty>().Where(a => a.month == month && a.year == year && a.productId == productId && a.companyId == companyId).Execute().FirstOrDefault();
            }
        }
        public VWPRD_ProductBounty[] GetVWPRD_ProductBountyByPeriodAndCompanyId(int month, int year, Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductBounty>().Where(a => a.month == month && a.year == year && a.companyId == companyId).Execute().ToArray();
            }
        }

    }
}
