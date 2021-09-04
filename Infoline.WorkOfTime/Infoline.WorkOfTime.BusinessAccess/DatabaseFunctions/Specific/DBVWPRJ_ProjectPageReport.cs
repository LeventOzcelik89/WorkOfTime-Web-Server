using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public VWPRJ_ProjectPageReport GetVWPRJ_ProjectPageReportSummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectPageReport>().Execute().FirstOrDefault();
            }
        }
    }
}