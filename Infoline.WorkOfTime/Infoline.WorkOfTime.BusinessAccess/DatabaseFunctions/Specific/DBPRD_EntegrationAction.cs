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
        public PRD_EntegrationAction GetPRD_EntegrationActionByRepetitive(string imei, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.Imei == imei && a.Quantity == 1).Execute().OrderByDescending(a=>a.created).FirstOrDefault();
            }
        }

        public PRD_EntegrationAction GetPRD_EntegrationActionByImei(string imei, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.Imei == imei).Execute().FirstOrDefault();
            }
        }
    }
}
