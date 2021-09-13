using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRD_CompanyBasedPriceDetail), "type")]
    public enum EnumPRD_CompanyBasedPriceType
    {
        [Description("Oran")]
        Oran = 0,
        [Description("Fiyat")]
        Fiyat,
    }
    partial class WorkOfTimeDatabase
    {
        public VWPRD_CompanyBasedPriceDetail[] GetVWPRD_CompanyBasedPriceDetailsByCompanyBasedId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_CompanyBasedPriceDetail>().Where(a => a.companyBasedPriceId == id).Execute().ToArray();
            }
        }

        public PRD_CompanyBasedPriceDetail[] GetPRD_CompanyBasedPriceDetailsByCompanyBasedId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_CompanyBasedPriceDetail>().Where(a => a.companyBasedPriceId == id).Execute().ToArray();
            }
        }

    }
    

}

