using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
   
    partial class WorkOfTimeDatabase
    {
        public UT_LocationConfig[] GetUT_LocationConfigByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_LocationConfig>().Where(x => x.id.In(ids)).Execute().ToArray();
            }
        }
    }
}
