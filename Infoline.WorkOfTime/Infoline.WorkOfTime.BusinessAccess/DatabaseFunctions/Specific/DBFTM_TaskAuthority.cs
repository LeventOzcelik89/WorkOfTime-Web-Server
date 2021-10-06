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

        public VWFTM_TaskAuthority[] GetVWFTM_TaskAuthorityByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWFTM_TaskAuthority>().Where(a => a.userId == userId).Execute().ToArray();
            }
        }

        public FTM_TaskAuthority GetFTM_TaskAuthorityByUserIdAndCustomerId(Guid userId,Guid customerId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<FTM_TaskAuthority>().Where(a => a.userId == userId && a.customerId == customerId).Execute().FirstOrDefault();
            }
        }

        public FTM_TaskAuthority GetFTM_TaskAuthorityByUserIdAndCustomerIdAndNotId(Guid userId, Guid customerId,Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<FTM_TaskAuthority>().Where(a => a.userId == userId && a.customerId == customerId && a.id != id).Execute().FirstOrDefault();
            }
        }

    }
}
