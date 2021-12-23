using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_CustomerUser), "customerType")]
    public enum EnumSV_CustomerUser
    {
        [Description("Bayi Personeli")]
        CompanyPerson = 0,
        [Description("Personel")]
        Person = 1,
        [Description("Diğer")]
        Other = 2,
    }
    partial class WorkOfTimeDatabase
    {

        public VWSV_CustomerUser GetVWSV_CustomerUserByServiceId(Guid serviceId, DbTransaction transaction = null)
        {
            using (var db = GetDB(transaction))
            {
                return db.Table<VWSV_CustomerUser>().Where(x => x.serviceId == serviceId).Execute().FirstOrDefault();
            }
        }

    }
}
