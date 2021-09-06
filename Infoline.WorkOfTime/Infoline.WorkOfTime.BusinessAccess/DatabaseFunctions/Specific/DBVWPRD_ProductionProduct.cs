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
        public VWPRD_ProductionProduct[] GetVWPRD_ProductionProductByProductIdAndProductionId(Guid productId,Guid productionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductionProduct>().Where(x => x.productId == productId &&x.productionId == productionId).Execute().ToArray();
            }
        }
    }
}
