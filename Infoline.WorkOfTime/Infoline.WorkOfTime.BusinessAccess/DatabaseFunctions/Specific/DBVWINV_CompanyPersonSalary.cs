using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{ 
     public partial class WorkOfTimeDatabase
    {
        public VWINV_CompanyPersonSalaryPageReport GetVWINV_CompanyPersonSalaryPageReportSummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonSalaryPageReport>().Execute().FirstOrDefault();
            }
        }

    }
}