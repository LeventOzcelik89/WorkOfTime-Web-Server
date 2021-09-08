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
        public VWPRD_ProductionOperation[] GetVWPRD_ProductionOperationByProductionId(Guid productionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductionOperation>().Where(x => x.productionId == productionId).Execute().ToArray();
            }
        }

        public VWPRD_ProductionOperation GetVWPRD_ProductionOperationByDataId(Guid dataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductionOperation>().Where(x => x.dataId == dataId).Execute().FirstOrDefault();
            }
        }
    }
}
