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
        public Guid[] GetCMP_CompanyTypeByCompanyIdTypesIds(Guid companyId)
        {
            using (var db = GetDB())
            {
                return db.Table<CMP_CompanyType>().Where(a => a.companyId == companyId && a.typesId != null).Select(a=>new CMP_CompanyType { id = a.id}).Execute().Select(x=>x.typesId.Value).ToArray();
            }
        }

        public CMP_CompanyType GetCMP_CompanyTypeByTypeId(Guid typeId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_CompanyType>().Where(a => a.typesId == typeId).Execute().FirstOrDefault();
            }
        }
        public CMP_CompanyType[] GetCMP_CompanyTypeByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_CompanyType>().Where(a => a.companyId == companyId).Execute().ToArray();
            }
        }

        public CMP_Types[] GetCMP_TypesLike(string name, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Types>().Where(a => a.typeName.ToLower().Contains(name.ToLower())).Execute().ToArray();
            }
        }

    }
}
