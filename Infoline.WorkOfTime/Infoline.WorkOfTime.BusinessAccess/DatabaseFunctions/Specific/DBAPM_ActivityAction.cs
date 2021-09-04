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

    [EnumInfo(typeof(APM_ActivityAction), "type")]
    public enum EnumAPM_ActivityActionType
    {
        [Description("Yeni Aktivite")]
        YeniAktivite = 0,
        [Description("Not Ekleme")]
        Not = 1,
        [Description("Aktivite Düzenleme")]
        AktiviteDuzenleme = 2,
    }


    partial class WorkOfTimeDatabase
    {
        public APM_ActivityAction[] GetAPM_ActivityActionByActivityId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<APM_ActivityAction>().Where(a => a.activityId == id).Execute().ToArray();
            }
        }

    }
}
