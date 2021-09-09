using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(VWPRD_CompanyBasedPrice), "EnumPRD_CompanyBasedPriceSellingType")]
    public enum EnumPRD_CompanyBasedPriceSellingType
    {
        [Description("Genel")]
        Genel = 0,
        [Description("Peşin")]
        Peşin,
        [Description("Taksitli")]
        Taksitli,
        [Description("Vadeli")]
        Vadeli
    }
    [EnumInfo(typeof(VWPRD_CompanyBasedPrice), "EnumPRD_CompanyBasedPriceType")]
    public enum EnumPRD_CompanyBasedPriceType
    {
        [Description("Oran")]
        Oran = 0,
        [Description("Fiyat")]
        Fiyat,
    }
    [EnumInfo(typeof(VWPRD_CompanyBasedPrice), "EnumPRD_CompanyBasedPriceConditionType")]
    public enum EnumPRD_CompanyBasedPriceConditionType
    {
        [Description("Genel")]
        Oran = 0,
        [Description("Satış Fiyatına Göre")]
        Fiyat
    }
    partial class WorkOfTimeDatabase
    {
        public VWPRD_CompanyBasedPriceDetail[] GetVWPRD_CompanyBasedPriceDetailByCompanyBasedPriceId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_CompanyBasedPriceDetail>().Where(a => a.companyBasedPriceId == id).Execute().ToArray();
            }



        }
}
