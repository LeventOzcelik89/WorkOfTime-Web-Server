using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SH_PagesRole), "Status")]
    public enum EnumSH_PagesRoleStatus
    {
        [Description("Aktif")]
        Aktif = 1,
        [Description("Pasif")]
        Pasif = 0
    }


    public partial class WorkOfTimeDatabase
    {

        public VWSH_PagesRole[] GetVWSH_PagesRoleByRoleId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_PagesRole>().Where(a => a.roleid == id).OrderBy(a => a.created).Execute().ToArray();
            }
        }
    }
}