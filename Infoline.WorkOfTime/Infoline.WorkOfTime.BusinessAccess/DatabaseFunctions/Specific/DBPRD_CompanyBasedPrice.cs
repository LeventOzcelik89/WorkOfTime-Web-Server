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
       public PRD_CompanyBasedPrice GetDBPRD_CompanyBasedPriceByAllAttributes(PRD_CompanyBasedPrice item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_CompanyBasedPrice>().Where(a => a.companyId == item.companyId && a.productId == item.productId && a.categoryId == item.categoryId && a.conditionType == item.conditionType && a.sellingType == item.sellingType&&a.id!=item.id).Execute().FirstOrDefault();
            }
        }
    }
}

