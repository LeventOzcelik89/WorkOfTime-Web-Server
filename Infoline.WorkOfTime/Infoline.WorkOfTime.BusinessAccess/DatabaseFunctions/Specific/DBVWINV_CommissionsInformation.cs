using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {

        public VWINV_CommissionsInformation GetVWINV_CommissionsInformationCommissionId(Guid commissionsId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsInformation>().Where(a => a.commissionsId == commissionsId).Execute().FirstOrDefault();
            }
        }
    }
}
