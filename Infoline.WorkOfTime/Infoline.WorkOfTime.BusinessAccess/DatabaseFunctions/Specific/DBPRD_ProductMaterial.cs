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
        public PRD_ProductMateriel GetPRD_ProductMaterielByProductIdAndMaterialId(Guid productId, Guid materialId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductMateriel>().Where(x => x.productId == productId && x.materialId == materialId).Execute().FirstOrDefault();
            }
        }

        public PRD_ProductMateriel[] GetPRD_ProductMaterielByMaterialId(Guid materialId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductMateriel>().Where(x => x.materialId == materialId).Execute().ToArray();
            }
        }

    }
}
