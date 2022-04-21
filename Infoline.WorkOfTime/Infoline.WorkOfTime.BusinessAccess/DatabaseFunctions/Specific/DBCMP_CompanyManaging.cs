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
        public CMP_CompanyManaging[] GetCMP_CompanyManagingByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_CompanyManaging>().Where(a => a.companyId == companyId).Execute().ToArray();
            }
        }
        public Guid[] GetCMP_CompanyManagingByCompanyIdUserIds(Guid companyId)
        {
            using (var db = GetDB())
            {
                return db.Table<CMP_CompanyManaging>().Where(a => a.companyId == companyId).Select(a => new CMP_CompanyManaging { id = a.id }).Execute().Where(x => x.userId.HasValue).Select(x => x.userId.Value).ToArray();
            }
        }
    }
}
