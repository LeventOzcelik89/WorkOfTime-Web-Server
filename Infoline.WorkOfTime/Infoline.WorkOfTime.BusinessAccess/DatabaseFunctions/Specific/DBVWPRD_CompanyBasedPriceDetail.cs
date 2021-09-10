using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    
    partial class WorkOfTimeDatabase
    {
        public VWPRD_CompanyBasedPriceDetail[] GetVWPRD_CompanyBasedPriceDetailsByCompanyBasedId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_CompanyBasedPriceDetail>().Where(a => a.companyBasedPriceId == id).Execute().ToArray();
            }
        }

    }
    

}

