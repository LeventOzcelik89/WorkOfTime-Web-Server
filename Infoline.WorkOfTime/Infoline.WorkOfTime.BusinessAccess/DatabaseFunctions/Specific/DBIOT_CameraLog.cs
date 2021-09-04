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
        public IOT_CameraLog[] GetIOT_CameraLogByIPAdress(string ip)
        {
            using (var db = GetDB())
            {
                return db.Table<IOT_CameraLog>().Where(a => a.ipAddress == ip).Execute().ToArray();
            }
        }
    }
}
