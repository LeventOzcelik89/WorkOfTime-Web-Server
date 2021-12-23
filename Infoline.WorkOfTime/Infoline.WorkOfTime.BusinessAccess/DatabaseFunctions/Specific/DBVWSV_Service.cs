using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_Service), "DeliveryType")]
    public enum EnumSV_ServiceDeliveryType
    {
        [Description("Elden Teslim")]
        HandedOver = 1,
        [Description("Kargo")]
        Cargo = 2,
        [Description("Diğer")]
        Other = 3,
    }
    [EnumInfo(typeof(SV_Service), "CustomerType")]
    public enum EnumSV_ServiceCustomerType
    {
        [Description("Bayi")]
        Company = 1,
        [Description("Müşteri")]
        Customer = 2,

    }
    partial class WorkOfTimeDatabase
    {
        public VWSV_Service GetVWSV_ServiceByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSV_Service>().Where(x => x.code == code).Execute().FirstOrDefault();
            }
        }
        public VWSV_Service GetVWSV_ServiceByCodeAndIdIsNotEqual(string code, Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSV_Service>().Where(x => x.code == code &&x.id==id).Execute().FirstOrDefault();
            }
        }
    }
}
