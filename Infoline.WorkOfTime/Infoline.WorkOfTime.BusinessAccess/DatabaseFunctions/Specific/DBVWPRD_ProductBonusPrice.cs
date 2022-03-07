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
        public VWPRD_ProductBonusPrice[] GetVWPRD_ProductBonusPriceByProductBonusId(Guid? bonusId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductBonusPrice>().Where(a => a.productBonusId == bonusId).Execute().ToArray();
            }
        }
    }
}
