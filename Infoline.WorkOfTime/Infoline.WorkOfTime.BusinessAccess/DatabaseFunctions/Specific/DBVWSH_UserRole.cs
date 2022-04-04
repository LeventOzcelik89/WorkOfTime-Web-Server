using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SH_UserRole), "status")]
    public enum EnumSH_UserRoleStatus
    {
        [Description("Aktif")]
        Aktif = 1,
        [Description("Pasif")]
        Pasif = 0
    }

    public partial class WorkOfTimeDatabase
    {

        public VWSH_UserRole[] GetVWSH_UserRoleByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_UserRole>().Where(x => x.userid == userId).Execute().ToArray();
            }
        }

        public VWSH_UserRole[] GetVWSH_UserRoleByRoleId(Guid roleid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_UserRole>().Where(x => x.roleid == roleid).Execute().ToArray();
            }
        }
    }
}