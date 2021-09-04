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
        public CMP_CompanyRelation GetCMP_CompanyRelationByCustomerIdAndCompanyId(Guid customerId,Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_CompanyRelation>().Where(a => a.customerId == customerId && a.companyId == companyId).Execute().FirstOrDefault();
            }
        }

    }
}
