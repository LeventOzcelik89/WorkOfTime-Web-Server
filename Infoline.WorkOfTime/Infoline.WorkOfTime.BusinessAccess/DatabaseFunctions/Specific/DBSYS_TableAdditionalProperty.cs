using System;
using System.Linq;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public SYS_TableAdditionalProperty[] GetSYS_TableAdditionalPropertyByDataIdAndDataTable(Guid dataId, string dataTable, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_TableAdditionalProperty>().Where(a => a.dataId == dataId && a.dataTable == dataTable).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

        public SYS_TableAdditionalProperty[] GetSYS_TableAdditionalPropertyByDataTable(string dataTable, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_TableAdditionalProperty>().Where(a => a.dataTable == dataTable).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }


    }
}
