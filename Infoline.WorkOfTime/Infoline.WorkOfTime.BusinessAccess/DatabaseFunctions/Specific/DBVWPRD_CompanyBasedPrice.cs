using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(VWPRD_CompanyBasedPrice), "SellingType")]
    public enum EnumVWPRD_CompanyBasedPriceSellingType
    {
        [Description("Peşin")]
        Advance,
        [Description("Taksitli")]
        Installement,
        [Description("Vadeli")]
        Term
    }
    [EnumInfo(typeof(VWPRD_CompanyBasedPrice), "ToFilterSpecified")]
    public enum EnumVWPRD_CompanyBasedPriceToFilterSpecified
    {
        [Description("Genel")]
        General,
        [Description("Satış Fiyatına Göre")]
        toSellingPrice,
        [Description("Adete Göre")]
        toNumber
    }
    partial class WorkOfTimeDatabase
    {
       
    }
}
