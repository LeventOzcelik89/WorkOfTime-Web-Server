using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public VWINV_PermitType_PageReport GetVWINV_PermitTypeSummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_PermitType_PageReport>().Execute().FirstOrDefault();
            }
        }
    }
}