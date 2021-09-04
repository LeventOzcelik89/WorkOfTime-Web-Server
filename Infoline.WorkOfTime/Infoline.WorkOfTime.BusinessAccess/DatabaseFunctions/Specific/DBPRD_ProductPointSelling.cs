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
        public PRD_ProductPointSelling GetPRD_ProductPointSellingByProductAndCompanyAndDateControl(Guid productId, DateTime startDate, DateTime endDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductPointSelling>().Where(a => a.productId == productId && ((a.endDate > startDate && a.startDate < startDate) || (a.endDate > endDate && a.startDate < endDate) || (a.startDate > startDate && a.startDate < endDate) || (a.endDate < endDate && a.endDate > startDate))).Execute().FirstOrDefault();
            }
        }


        public PRD_ProductPointSelling[] GetPRD_ProductPointSellingByProductId(Guid productId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductPointSelling>().Where(a => a.productId == productId).Execute().ToArray();
            }
        }

    }
}
