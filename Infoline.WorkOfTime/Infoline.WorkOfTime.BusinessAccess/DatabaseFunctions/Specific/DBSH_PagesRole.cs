using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {

        public SH_PagesRole[] GetSH_PagesRoleByRoleId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_PagesRole>().Where(a => a.roleid == id).OrderBy(a => a.created).Execute().ToArray();
            }
        }
    }
}