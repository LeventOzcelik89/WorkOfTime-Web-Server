using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(CRM_ContactUser), "UserType")]
    public enum EnumCRM_ContactUserUserType
    {
        [Description("Bizim Personelimiz")]
        OwnerUser = 0,
        [Description("Kanal Personeli")]
        ChannelUser = 1,
        [Description("Müşteri Personeli")]
        CustomerUser = 2
    }

    public partial class WorkOfTimeDatabase
    {
        public CRM_ContactUser[] GetCRM_ContactUserByContactId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_ContactUser>().Where(a => a.ContactId == id).Execute().ToArray();
            }
        }


    }
}


