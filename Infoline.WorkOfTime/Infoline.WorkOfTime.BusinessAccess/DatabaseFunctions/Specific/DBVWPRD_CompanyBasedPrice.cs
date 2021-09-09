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
        Genel = 0,
        [Description("Minimum Satış Fiyatına Göre")]
        Fiyat,
        [Description("Minimum Adete Göre")]
        Adet
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
    partial class WorkOfTimeDatabase
    {
        public VWPRD_CompanyBasedPriceDetail GetVWPRD_CompanyBasedDetailIsExistBefore(VWPRD_CompanyBasedPriceDetail item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_CompanyBasedPriceDetail>().Where(a =>
                a.companyId==item.companyId&&
                a.productId==item.productId&&
                a.categoryId==item.categoryId&&
                a.conditionType==item.conditionType&&
                a.sellingType==item.sellingType&&
                a.minCondition==item.minCondition&&
                a.monthCount==item.monthCount&&
                a.companyBasedPriceId==item.companyBasedPriceId
                &&a.id!=item.id)
                .Execute().FirstOrDefault();
            }
        }

    }

}

