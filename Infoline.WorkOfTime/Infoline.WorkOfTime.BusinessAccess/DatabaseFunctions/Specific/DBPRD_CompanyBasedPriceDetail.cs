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
       public PRD_CompanyBasedPriceDetail[] GetPRD_CompanyBasedPriceDetailWithSameData(PRD_CompanyBasedPriceDetail item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_CompanyBasedPriceDetail>().Where(a => a.companyBasedPriceId == item.companyBasedPriceId && 
                                                                     a.minCondition == item.minCondition && 
                                                                     a.type == item.type && 
                                                                     a.discount == item.discount && 
                                                                     a.price == item.price && 
                                                                     a.monthCount == item.monthCount&&a.id!=item.id).Execute().ToArray();
            }
        }
    }
}

