using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRD_CompanyBasedPrice), "sellingType")]
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

    [EnumInfo(typeof(PRD_CompanyBasedPrice), "conditionType")]
    public enum EnumPRD_CompanyBasedPriceConditionType
    {
        [Description("Genel")]
        Genel = 0,
        [Description("Minimum Satış Fiyatına Göre")]
        Fiyat,
        [Description("Minimum Adete Göre")]
        Adet
    }

    [EnumInfo(typeof(PRD_CompanyBasedPrice), "companyType")]
    public enum EnumPRD_CompanyBasedPriceCompanyType
    {
        [Description("Tüm Şirketlere")]
        AllCompanies = 0,
        [Description("Seçili Şirkete")]
        SelectedCompany,

    }
    
    [EnumInfo(typeof(PRD_CompanyBasedPrice), "productType")]
    public enum EnumPRD_CompanyBasedPriceProductType
    {
        [Description("Tüm Ürünlere")]
        AllProducts = 0,
        [Description("Seçili Kategoriye")]
        SelectedCategory,
        [Description("Seçili Ürüne")]
        SelectedProduct,

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

