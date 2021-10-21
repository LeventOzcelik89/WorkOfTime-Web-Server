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
        public VWCMP_CompanyType[] GetVWCMP_CompanyTypeByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_CompanyType>().Where(a => a.companyId == companyId).Execute().ToArray();
            }
        }
    }
}
