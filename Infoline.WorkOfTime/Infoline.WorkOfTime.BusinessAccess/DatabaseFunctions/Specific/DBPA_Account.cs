using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(PA_Account), "type")]
    public enum EnumPA_AccountType
    {
        [Description("Kasa")]
        Kasa = 0,
        [Description("Banka")]
        Banka = 1,
    }


    [EnumInfo(typeof(PA_Account), "status")]
    public enum EnumPA_AccountStatus
    {
        [Description("Pasif")]
        Pasif = 0,
        [Description("Aktif")]
        Aktif = 1,
    }
    partial class WorkOfTimeDatabase
    {
        public PA_Account[] GetPA_AccountByDataId(Guid dataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Account>().Where(a => a.dataId == dataId).Execute().ToArray();
            }
        }

        public PA_Account[] GetPA_AccountIsDefaultByDataId(Guid DataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Account>().Where(a => a.dataId == DataId && a.isDefault == true).Execute().ToArray();
            }
        }
    }
}
