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
        public VWPRD_DistributionPlanRelation[] GetVWPRD_DistributionPlanRelationByDistributionPlanId(Guid distributionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_DistributionPlanRelation>().Where(a => a.id == distributionId).OrderBy(a => a.created).Execute().ToArray();
            }
        }
    }
}
