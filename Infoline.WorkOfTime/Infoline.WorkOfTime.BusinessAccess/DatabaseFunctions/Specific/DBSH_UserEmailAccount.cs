using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [BusinessAccess.EnumInfo(typeof(SH_UserEmailAccount), "mailType")]
    public enum EnumSH_UserEmailAccountmailType
    {
        [Description("Outlook")]
        Outlook = 0
    }

    [BusinessAccess.EnumInfo(typeof(SH_UserEmailAccount), "isDefault")]
    public enum EnumSH_UserEmailAccountisDefault
    {
        [Description("Evet")]
        Evet = 0,
        [Description("Hayır")]
        Hayir = 1
    }


    public partial class WorkOfTimeDatabase
    {
        public SH_UserEmailAccount GetSH_UserEmailAccountByUserId(Guid userid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserEmailAccount>().Where(a => a.userid == userid).Execute().FirstOrDefault();
            }
        }

        public SH_UserEmailAccount GetSH_UserEmailAccountByEmail(string email, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserEmailAccount>().Where(a => a.email == email).Execute().FirstOrDefault();
            }
        }

        public SH_UserEmailAccount GetSH_UserEmailAccountByUserIdİsDefaultTrue(Guid userid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserEmailAccount>().Where(a => a.userid == userid && a.isDefault == (int)EnumSH_UserEmailAccountisDefault.Evet).Execute().FirstOrDefault();
            }
        }
    }
}
