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
        public PRD_EntegrationAction[] GetPRD_EntegrationActionBySerialNumbersOrImeis(string[] imeis,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.SerialNo.In(imeis) ||a.Imei.In(imeis)).Execute().ToArray();
            }
        }
        public PRD_EntegrationAction GetPRD_EntegrationActionBySerialNumbersOrImei(string imei, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.SerialNo==imei || a.Imei==imei).Execute().FirstOrDefault();
            }
        }

        public VWPRD_EntegrationAction[] GetVWPRD_EntegrationActionByDistrubutorId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_EntegrationAction>().Where(a => a.DistributorId == id).Execute().ToArray();
            }
        }
        public VWPRD_EntegrationAction[] GetVWPRD_EntegrationActionBySellerId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_EntegrationAction>().Where(a => a.CustomerOperatorId == id).Execute().ToArray();
            }
        }
    }
}
