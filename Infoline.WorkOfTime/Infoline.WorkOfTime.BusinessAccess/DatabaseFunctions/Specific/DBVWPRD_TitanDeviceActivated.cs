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
        public VWPRD_TitanDeviceActivated GetVWPRD_TitanDeviceActivatedByImei(string imei1, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_TitanDeviceActivated>().Where(a => a.IMEI1 == imei1).Execute().FirstOrDefault();
            }
        }

    }
}
